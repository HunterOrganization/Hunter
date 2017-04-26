using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.GameCore;
using Hunter.TradeSystem;
using Hunter.EventSystem;
using Hunter.UI;
using Hunter.Fight;
using Hunter.EquipSystem;
using Hunter.SettingMgr;
using Basic.ResourceMgr;

namespace Hunter.HunterInfo
{
    public class PlayerManager : MonoBehaviour
    {

        #region 属性配置
        [SerializeField]
        float maxLife;
        [SerializeField]
        float maxHot;
        [SerializeField]
        float maxHunger;
        [SerializeField]
        float maxAlone;
        [SerializeField]
        float moveSpeed;

        [SerializeField]
        float consumeHot = -0.01f; // 每米消耗
        [SerializeField]
        float consumeHunger = -0.02f;
        [SerializeField]
        float addAlone = 0.01f;
        #endregion

        // 维护食物信息
        FoodSettingPro foodSettingPro;
        // 角色道具
        Dictionary<string, ItemInfo> items;
        // 角色属性
        PlayerProperties properties;
        
        #region 系统事件
        void Start()
        {
            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(RequestItemByTrade), HandleRequestItemsByTrade);
            MessagingSystem.Instance.AttachListener(typeof(RequestTradeFinish), HandleTrandeFinish);
            MessagingSystem.Instance.AttachListener(typeof(UpdateHunterDist), HandleUpdateHunterDist);
            MessagingSystem.Instance.AttachListener(typeof(FightConsumeMsg), HandleFightConsume);
            MessagingSystem.Instance.AttachListener(typeof(ChangeItemMsg), HandleChangeItem);
            MessagingSystem.Instance.AttachListener(typeof(RequestOpenBagMsg), HandleOpenBag);
            MessagingSystem.Instance.AttachListener(typeof(UsedItemMsg), HandleUsedItem);
            MessagingSystem.Instance.AttachListener(typeof(RequestAddItemMsg), HandleRequestAddItem);
            // -------------------------------------------------------------------------------------------------- //

            Invoke("LoadData", 1.0f);
        }

        void OnDestory()
        {
            if (MessagingSystem.IsAlive)
            {
                MessagingSystem.Instance.DetachListener(typeof(RequestItemByTrade), HandleRequestItemsByTrade);
                MessagingSystem.Instance.DetachListener(typeof(RequestTradeFinish), HandleTrandeFinish);
                MessagingSystem.Instance.DetachListener(typeof(UpdateHunterDist), HandleUpdateHunterDist);
                MessagingSystem.Instance.DetachListener(typeof(FightConsumeMsg), HandleFightConsume);
                MessagingSystem.Instance.DetachListener(typeof(ChangeItemMsg), HandleChangeItem);
                MessagingSystem.Instance.DetachListener(typeof(RequestOpenBagMsg), HandleOpenBag);
                MessagingSystem.Instance.DetachListener(typeof(UsedItemMsg), HandleUsedItem);
                MessagingSystem.Instance.DetachListener(typeof(RequestAddItemMsg), HandleRequestAddItem);
            }
        }
        #endregion

        #region 逻辑功能

        void LoadData()
        {
            // 测试
            items = new Dictionary<string, ItemInfo>(); // 可能从存储中获取
            items.Add("Yeshoupi1", new ItemInfo("Yeshoupi1", 2));
            items.Add("Lieqiang2", new ItemInfo("Lieqiang2", 2));
            items.Add("Lieqiang1", new ItemInfo("Lieqiang1", 2));
            items.Add("Qiangtao1", new ItemInfo("Qiangtao1", 2));
            items.Add("Yeguo1", new ItemInfo("Yeguo1", 2));
            items.Add("Zidan1", new ItemInfo("Zidan1", 220));

            // 读取setting
            IResourceMgr rm = new ResourceMgr();
            foodSettingPro = new FoodSettingPro(rm);
            // 生成属性
            properties = new PlayerProperties(maxLife, maxHot, maxHunger, 0, maxAlone);

            // 读取玩数据后，通知修改猎人界面
            MessagingSystem.Instance.QueueMessage(new UpdateInfoMsg(properties));
            // 通知开始速度计算
            MessagingSystem.Instance.QueueMessage(new UpdateMoveSpeed(moveSpeed));
        }

        #endregion

        #region 消息处理事件
        bool HandleRequestAddItem(BaseMessage bm)
        {
            RequestAddItemMsg msg = bm as RequestAddItemMsg;

            if(msg.addCount > 0)
            {
                if (items.ContainsKey(msg.itemId))
                {
                    items[msg.itemId].count += msg.addCount;
                }
                else
                {
                    ItemInfo newItem = new ItemInfo(msg.itemId, msg.addCount);
                    items.Add(msg.itemId, newItem);
                }
            }
            else
            {
                if (items.ContainsKey(msg.itemId))
                {
                    int leftCount = items[msg.itemId].count + msg.addCount;
                    items[msg.itemId].count = leftCount >= 0? leftCount:0;
                }
                else
                {
                    Debug.Log("错误发生于 PlayerManager/HandleRequestAddItem, 错误的扣除不存在的道具");
                }
            }
            return true;
        }

        bool HandleRequestItemsByTrade(BaseMessage bm)
        {
            RequestItemByTrade ri = bm as RequestItemByTrade;

            Dictionary<string, int> requestItems = new Dictionary<string, int>();
            foreach (var id in items)
            {
                if (id.Value.count > 0)
                    requestItems.Add(id.Key, id.Value.count);
            }

            MessagingSystem.Instance.QueueMessage(new UpdateTradeStepMsg(TradeStep.ReportItemsInfo, requestItems));
            return true;
        }

        bool HandleTrandeFinish(BaseMessage bm)
        {
            RequestTradeFinish msg = bm as RequestTradeFinish;

            foreach (var item in msg.goodsChange)
            {
                if (items.ContainsKey(item.Key)) {
                    items[item.Key].count += item.Value;
                }
                else if (item.Value > 0)
                {
                    items[item.Key] = new ItemInfo(item.Key, item.Value);
                }
                else
                {
                    Debug.Log(string.Format("错误在于 HunterManager/HandleTrandeFinish, 猎人并没有拥有物品 {0}", item.Key));
                }
            }
            return true;
        }

        bool HandleUpdateHunterDist(BaseMessage bm)
        {
            UpdateHunterDist msg = bm as UpdateHunterDist;

            // （饱腹值）*10%+（温度）*7%-（孤独）*16%=当前回血量【取整】。
            properties.Life += (properties.Hunger * 0.1f + properties.Hot * 0.07f - properties.Alone * 0.16f);
            properties.Life = properties.Life > 100 ? 100 : properties.Life;
            properties.Hot += (msg.updateDist * consumeHot);
            properties.Hunger += (msg.updateDist * consumeHunger);
            properties.Alone += (msg.updateDist * addAlone);

            // 通知UI更新猎人信息
            MessagingSystem.Instance.QueueMessage(new UpdateInfoMsg(properties));
            return true;
        }
        bool HandleFightConsume(BaseMessage bm)
        {
            FightConsumeMsg msg = bm as FightConsumeMsg;

            if (properties.Life < msg.life)
            {
                // 游戏结束
                Debug.Log("游戏结束！");
                MessagingSystem.Instance.QueueMessage(new GameOver());
                return true;
            }
            else
            {
                properties.Life -= msg.life;
            }

            int ammoCount = items["Zidan1"].count;

            if (ammoCount < msg.ammo)
            {
                // 游戏结束
                Debug.Log("游戏结束！");
                MessagingSystem.Instance.QueueMessage(new GameOver());
                return true; 
            }
            else
            {
                items["Zidan1"].count -= msg.ammo;
            }
            // 通知UI更新猎人信息
            MessagingSystem.Instance.QueueMessage(new UpdateInfoMsg(properties));
            return true;
        }

        bool HandleChangeItem(BaseMessage bm)
        {
            ChangeItemMsg msg = bm as ChangeItemMsg;

            foreach(var v in msg.changeList)
            {
                items[v.Key].count += v.Value;
                if(items[v.Key].count < 0)
                {
                    items[v.Key].count = 0;
                    Debug.Log("错误发生于 PlayerManager/HandleChangeItem, 错误的道具修改");
                }
            }
            return true;
        }

        bool HandleOpenBag(BaseMessage bm)
        {
            if (items == null) return true;

            MessagingSystem.Instance.QueueMessage(new RequestBagInfoMsg(UsedItem.OpenBag, items));
            return true;
        }

        bool HandleUsedItem(BaseMessage bm)
        {
            UsedItemMsg msg = bm as UsedItemMsg;

            if(!items.ContainsKey(msg.idItem))
            {
                Debug.Log(string.Format("错误发生于 PlayerManager/HandleUsedItem，角色并没有该物品{0}", msg.idItem));
                return true;
            }

            IFoodSetting setting = foodSettingPro.GetItem(msg.idItem);

            if (setting == null)
            {
                Debug.Log(string.Format("错误发生于 PlayerManager/HandleUsedItem， 错误的食物ID{0}", msg.idItem));
                return true;
            }

            items[msg.idItem].count -= 1;

            properties.Life += setting.Life;
            properties.Hot += setting.Hot;
            properties.Alone += setting.Alone;
            properties.Hunger += setting.Hunger;

            MessagingSystem.Instance.QueueMessage(new UpdateInfoMsg(properties));
            MessagingSystem.Instance.QueueMessage(new BottomDialogMessage(setting.Describe));
            return true;
        }
        #endregion
    }

    class ItemInfo // 用于存储的背包物品类
    {
        public string itemId;
        public int count;
        public ItemInfo(string id, int c)
        {
            itemId = id;
            count = c;
        }
    }

    #region 消息类
    class RequestAddItemMsg: BaseMessage
    {
        public readonly int addCount;
        public readonly string itemId;
        
         public RequestAddItemMsg(int c, string id)
        {
            addCount = c;
            itemId = id;
        }
    }


    class RequestItemByTrade : BaseMessage { }

    class RequestTradeFinish : BaseMessage
    {
        public readonly Dictionary<string, int> goodsChange;

        public RequestTradeFinish(Dictionary<string, int> gc)
        {
            goodsChange = gc;
        }
    }

    class UsedItemMsg: BaseMessage
    {
        public readonly string idItem;
        public UsedItemMsg(string id)
        {
            idItem = id;
        }
    }
    class UpdateMoveSpeed : BaseMessage
    {
        public readonly float speed;

        public UpdateMoveSpeed(float s)
        {
            speed = s;
        }
    }

    class RequestBagInfoMsg: BaseMessage
    {
        public readonly UsedItem usedItem;
        public readonly Dictionary<string, ItemInfo> bagList;
        public readonly Dictionary<string, IEquipSetting> equipList;

        public RequestBagInfoMsg(UsedItem ui, Dictionary<string, IEquipSetting> el)
        {
            usedItem = ui;
            equipList = el;
        }

        public RequestBagInfoMsg(UsedItem ui, Dictionary<string, ItemInfo> bl)
        {
            usedItem = ui;
            bagList = bl;
        }
    }

    class ChangeItemMsg: BaseMessage
    {
        public readonly Dictionary<string, int> changeList;

        public ChangeItemMsg(Dictionary<string, int> cl)
        {
            changeList = cl;
        }
    }

    class RequestOpenBagMsg: BaseMessage { }

    class GameOver: BaseMessage{}
    #endregion
}