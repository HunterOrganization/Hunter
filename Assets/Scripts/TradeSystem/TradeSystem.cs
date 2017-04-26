using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.GameCore;
using Hunter.HunterInfo;
using Hunter.UI;
using Basic.ResourceMgr;
using Hunter.SettingMgr;
using System;
using System.Text;
using Hunter.EventSystem;
using Hunter.Tools;
using Hunter.EquipSystem;

namespace Hunter.TradeSystem
{
    enum TradeStep
    {
        BeginTrade,
        ReportTradeAI,
        ReportItemsInfo,
        ConfirmTrade,
    }

    enum UsedItem
    {
        OpenBag,
        EquipData,
        UsedItem,
    }

    public class TradeSystem : MonoBehaviour
    {
        // 物品列表<物品ID, 物品信息>(暂时由交易系统负责道具管理，之后需要再进行修改)
        ItemsSettingPro itemsSetting;
        // 物品价值观念列表<价值观ID, 价值类对象>
        NpcGoodsValueSettingPro valuesSetting;

        // 缓存数据(TODO 需要构思一个好的上下文缓存机制)
        bool isSpoils;
        string valueId;
        int eventWeight;
        Dictionary<string, int> demandGoodsList = new Dictionary<string, int>(); // 需求列表 
        List<GoodsInfo> ownGoodsDict = new List<GoodsInfo>(); // 持有列表
        List<GoodsInfo> hunterGoods = new List<GoodsInfo>(); // 猎人物品
        List<BagItemInfo> bagList; // 猎人背包列表

        #region 系统事件
        void Start()
        {
            // 读取表格数据（npc商品价值表、商品表）
            IResourceMgr rm = new ResourceMgr();
            valuesSetting = new NpcGoodsValueSettingPro(rm);
            itemsSetting = new ItemsSettingPro(rm);

            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(UpdateTradeStepMsg), HandleUpdateTradeStep); // 返回猎人道具数据
            MessagingSystem.Instance.AttachListener(typeof(RequestBagInfoMsg), HandleRequestBagInfo); // 返回背包信息
            // -------------------------------------------------------------------------------------------------- //
        }

        void OnDestory()
        {
            if (MessagingSystem.IsAlive)
            {
                MessagingSystem.Instance.DetachListener(typeof(UpdateTradeStepMsg), HandleUpdateTradeStep);
                MessagingSystem.Instance.DetachListener(typeof(RequestBagInfoMsg), HandleRequestBagInfo);
            }
        }
        #endregion


        #region 消息处理事件
        /// <summary>
        /// 处理整个交易过程的所有功能
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        bool HandleUpdateTradeStep(BaseMessage bm)
        {
            UpdateTradeStepMsg msg = bm as UpdateTradeStepMsg;

            switch (msg.tradeStep)
            {
                case TradeStep.BeginTrade:
                    Debug.Log(msg.tardeArgs);
                    // 参数列表是 持有ID， 持有数量，需求ID，需求数量，价值观ID，事件参数
                    List<int> args = Lib.GetIntInString(msg.tardeArgs);
                    if(args.Count != 6)
                    {
                        Debug.Log("错误发生于 TradeSystem/HandleUpdateTradeStep 中，CSV表格配置有误！");
                    }
                    valueId = args[4].ToString();
                    eventWeight = args[5];
                    isSpoils = msg.isSpoils;

                    // 1,请求获取交易AI数据
                    MessagingSystem.Instance.QueueMessage(new RequestNpcDataMsg(
                        args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString()));
                    demandGoodsList.Clear();
                    ownGoodsDict.Clear();
                    hunterGoods.Clear();
                    break;

                case TradeStep.ReportTradeAI:
                    demandGoodsList = msg.demandGoods;
                    foreach (var goods in msg.ownGoods)
                    {
                        if (itemsSetting.ContainsKey(goods.Key))
                        {
                            ownGoodsDict.Add(new GoodsInfo(goods.Key, itemsSetting.GetItem(goods.Key).Name, goods.Value, itemsSetting.GetItem(goods.Key).Weight));
                        }
                        else
                        {
                            Debug.Log(string.Format("错误在于 TradeSystem/HandleNpcTradeData, 不存在的物品id {0}", goods.Key));
                        }
                    }
                    // 2，请求角色道具信息
                    MessagingSystem.Instance.QueueMessage(new RequestItemByTrade());
                    break;

                case TradeStep.ReportItemsInfo:
                    foreach (var goods in msg.hunterItems)
                    {
                        if (itemsSetting.ContainsKey(goods.Key) && itemsSetting.GetItem(goods.Key).Value > 0)
                        {
                            hunterGoods.Add(new GoodsInfo(goods.Key, itemsSetting.GetItem(goods.Key).Name, goods.Value, itemsSetting.GetItem(goods.Key).Weight));
                        }
                        else if(!itemsSetting.ContainsKey(goods.Key))
                        {
                            Debug.Log(string.Format("错误在于 TradeSystem/handleReportItems, 不存在的物品id {0}", goods.Key));
                        }
                    }
                    // 3，发送用户和持有物品信息发送给UI层
                    MessagingSystem.Instance.QueueMessage(new ReportTradeData(ownGoodsDict, hunterGoods));
                    break;

                case TradeStep.ConfirmTrade:
                    bool canTrade = false;
                    if (!isSpoils)
                    {
                        ReportTradeState rts = CheckHunterGoods(hunterGoods);
                        // 交易不满足
                        if (rts != null)
                        {
                            MessagingSystem.Instance.QueueMessage(rts);
                            return true;
                        }

                        // 计算猎人物品总价值
                        float hunterCost = GetGoodsCost(hunterGoods);
                        // 计算Npc物品总价值
                        float npcGoodsValues = GetGoodsCost(ownGoodsDict);
                        canTrade = hunterCost >= npcGoodsValues;
                    }
                    else
                    {
                        canTrade = true;
                    }
                    
                    if (canTrade)
                    {
                        StringBuilder obtainTip = new StringBuilder();
                        StringBuilder giveTip = new StringBuilder();
                        StringBuilder resultTip = new StringBuilder();
                        Dictionary<string, int> goodsChange = new Dictionary<string, int>();
                        
                        // 计算所得
                        foreach (var goods in hunterGoods)
                        {
                            if (goods.giveCount > 0)
                            {
                                goodsChange[goods.goodsId] = -goods.giveCount;
                                giveTip.Append(string.Format("{0}({1})", goods.name, goods.giveCount));
                            }
                        }
                        if (giveTip.Length > 0)
                            resultTip.Append("失去了" + giveTip.ToString());

                        // 通知猎人数据 修改道具信息
                        foreach (var goods in ownGoodsDict)
                        {
                            if (goods.giveCount > 0)
                            {
                                goodsChange[goods.goodsId] = goodsChange.ContainsKey(goods.goodsId) ? goodsChange[goods.goodsId] + goods.giveCount : goods.giveCount;
                                obtainTip.Append(string.Format("{0}({1})", goods.name, goods.giveCount));
                            }
                        }
                        if (obtainTip.Length > 0)
                        {
                            resultTip.Append(resultTip.Length > 0? "，获得了": "获得了");
                            resultTip.Append(obtainTip.ToString());
                        }

                        // 通知显示在消息框
                        MessagingSystem.Instance.QueueMessage(new BottomDialogMessage(resultTip.ToString()));
                        // 通知修改猎人信息
                        MessagingSystem.Instance.QueueMessage(new RequestTradeFinish(goodsChange));
                        // 交易完成关闭界
                        MessagingSystem.Instance.QueueMessage(new ReportTradeState(TradeResultState.Finish, ""));
                    }
                    else
                    {
                        MessagingSystem.Instance.QueueMessage(new ReportTradeState(TradeResultState.NotDelighted, ""));
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// 由于贸易系统暂时拥有道具管理职能，因此获取背包道具信息时需要来这里获取
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        bool HandleRequestBagInfo(BaseMessage bm)
        {
            RequestBagInfoMsg msg = bm as RequestBagInfoMsg;

            switch (msg.usedItem)
            {
                case UsedItem.OpenBag:
                    bagList = new List<BagItemInfo>();
                    foreach (var item in msg.bagList)
                    {
                        IItemsSetting itemSetting = itemsSetting.GetItem(item.Value.itemId);
                        if(itemsSetting == null)
                        {
                            Debug.Log(string.Format("错误发生于 TradeSystem/ HandleRequestBagInfo，背包中存在无效的物品ID{0}", item.Value.itemId));
                            continue;
                        }
                        
                        if (item.Value.count != 0)
                        {
                            BagItemInfo bagInfo = new BagItemInfo(itemSetting, item.Value.count);
                            bagList.Add(bagInfo);
                        }
                    }
                    MessagingSystem.Instance.QueueMessage(new RequestPlayerEquips());
                    break;
                case UsedItem.EquipData:
                    // 获得装备列表
                    foreach(var equip in msg.equipList)
                    {
                        var itemSetting = itemsSetting.GetItem(equip.Value.Id);
                        BagItemInfo bagInfo = new BagItemInfo(itemSetting, 0);
                        bagList.Insert(0, bagInfo);
                    }

                    // 发送给至UI
                    MessagingSystem.Instance.QueueMessage(new OpenBagMsg(bagList));
                    break;
                case UsedItem.UsedItem:
                    break;
            }
            return true;
        }
        #endregion

        #region 逻辑功能
        float GetGoodsCost(List<GoodsInfo> goodsList)
        {
            INpcGoodsValueSetting gvsetting = valuesSetting.GetItem(valueId);
            Type gvType = gvsetting.GetType();
            var properties = gvType.GetProperties();

            float cost = 0.0f;
            foreach (var goods in goodsList)
            {
                cost += (float)gvType.GetProperty(goods.goodsId).GetValue(gvsetting, null) *
                     itemsSetting.GetItem(goods.goodsId).Value * goods.giveCount * (eventWeight / 100.0f); 
            }
            Debug.Log("最终价格：" + cost);
            return cost;
        }

        ReportTradeState CheckHunterGoods(List<GoodsInfo> hunterGoods)
        {
            foreach(var goods in hunterGoods)
            {
                if (goods.giveCount > 0 && !demandGoodsList.ContainsKey(goods.goodsId))
                {
                    return new ReportTradeState(TradeResultState.NotNeed, itemsSetting.GetItem(goods.goodsId).Name);
                }

                if(demandGoodsList.ContainsKey(goods.goodsId) && demandGoodsList[goods.goodsId] < goods.giveCount)
                {
                    return new ReportTradeState(TradeResultState.Enough, itemsSetting.GetItem(goods.goodsId).Name);
                }
            }

            return null;
        }
        #endregion
    }

    #region 道具信息数据类型
    /// <summary>
    /// 用于交易的道具信息类型
    /// </summary>
    public class GoodsInfo
    {
        public string goodsId;
        public string name;
        public int count;
        public int giveCount = 0;
        public float weight;
        public GoodsInfo(string id, string n, int c, float w)
        {
            goodsId = id;
            name = n;
            count = c;
            weight = w;
        }
    }

    /// <summary>
    /// 用于背包的道具数据类型
    /// </summary>
    public class BagItemInfo
    {
        public readonly IItemsSetting itemSetting;
        public readonly int count;
        public BagItemInfo(IItemsSetting setting, int c)
        {
            itemSetting = setting;
            count = c;
        }
    }
    #endregion

    #region 消息类
    class UpdateTradeStepMsg : BaseMessage
    {
        public readonly bool isSpoils = false;
        public readonly TradeStep tradeStep;
        public readonly string tardeArgs;
        public readonly Dictionary<string, int> hunterItems;
        public readonly Dictionary<string, int> demandGoods;
        public readonly Dictionary<string, int> ownGoods;
        public UpdateTradeStepMsg(TradeStep ts, string ta, bool s = false)
        {
            tradeStep = ts;
            tardeArgs = ta;
            isSpoils = s;
        }

        public UpdateTradeStepMsg(TradeStep ts, Dictionary<string, int> dg, Dictionary<string, int> og)
        {
            tradeStep = ts;
            demandGoods = dg;
            ownGoods = og;
        }

        public UpdateTradeStepMsg(TradeStep ts, Dictionary<string, int> hi)
        {
            tradeStep = ts;
            hunterItems = hi;
        }

        public UpdateTradeStepMsg(TradeStep ts)
        {
            tradeStep = ts;
        }

    }
    #endregion
}