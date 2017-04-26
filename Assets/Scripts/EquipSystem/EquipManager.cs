using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.GameCore;
using Hunter.Fight;
using Hunter.HunterInfo;
using Hunter.TradeSystem;
using Hunter.SettingMgr;
using Basic.ResourceMgr;
using Hunter.UI;

namespace Hunter.EquipSystem
{
    enum EquipState
    {
        Equip,
        Unequip,
    }

    public class EquipManager : MonoBehaviour
    {
        // 维护装备配置列表
        EquipSettingPro settingPro;

        // 维护玩家装备列表<装备类型，装备ID>
        Dictionary<string, IEquipSetting> playerEquips = new Dictionary<string, IEquipSetting>();

        #region 系统事件
        void Start()
        {
            LoadSetting();
            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(RequestPlayerFightInfo), HandleRequestPlayerFightInfo);
            MessagingSystem.Instance.AttachListener(typeof(RequestPlayerEquips), HandleRequestPlayerEquips);
            MessagingSystem.Instance.AttachListener(typeof(ChangeEquipMsg), HandleChangeEquip);
            // -------------------------------------------------------------------------------------------------- //
        }

        void OnDectory()
        {
            MessagingSystem.Instance.DetachListener(typeof(RequestPlayerFightInfo), HandleRequestPlayerFightInfo);
            MessagingSystem.Instance.DetachListener(typeof(RequestPlayerEquips), HandleRequestPlayerEquips);
            MessagingSystem.Instance.DetachListener(typeof(ChangeEquipMsg), HandleChangeEquip);
        }
        #endregion

        #region 逻辑事件
        void LoadSetting()
        {
            ResourceMgr rm = new ResourceMgr();
            settingPro = new EquipSettingPro(rm);
        }

        #endregion

        #region 消息处理事件
        bool HandleRequestPlayerFightInfo(BaseMessage bm)
        {
            RequestPlayerFightInfo msg = bm as RequestPlayerFightInfo;
            int aTotal = 0, dToatal = 0;
            // 计算总攻击力
            foreach(var equip in playerEquips)
            {
                aTotal += equip.Value.Attack;
                dToatal += equip.Value.Defense;
            }
            MessagingSystem.Instance.QueueMessage(new UpdateFightStepMsg(FightState.GetPlayerInfo, aTotal, dToatal));
            return true;
        }

        bool HandleRequestPlayerEquips(BaseMessage bm)
        {
            MessagingSystem.Instance.QueueMessage(new RequestBagInfoMsg(UsedItem.EquipData, playerEquips));
            return true;
        }

        bool HandleChangeEquip(BaseMessage bm)
        {
            ChangeEquipMsg msg = bm as ChangeEquipMsg;

            Dictionary<string, int> changeDict = new Dictionary<string, int>();
            IEquipSetting equipSetting = settingPro.GetItem(msg.equipId);
            switch (msg.equipState)
            {
                case EquipState.Equip:
                    string desc = "你";
                    if(playerEquips.ContainsKey(equipSetting.EType) &&
                        playerEquips[equipSetting.EType].Id == msg.equipId)
                    {
                        desc += string.Format("紧张摸了一下身上的{0}，安心了下来。", msg.equipName);
                    }
                    else if(playerEquips.ContainsKey(equipSetting.EType))
                    {
                        desc += playerEquips[equipSetting.EType].UnequipDesc;
                        desc += "，然后" + equipSetting.EquipDesc + ".";
                        string uneuquipId = playerEquips[equipSetting.EType].Id;
                        playerEquips[equipSetting.EType] = equipSetting;

                        // 通知背包道具，删除一个装备物品，增加一个卸载物品
                        changeDict.Add(equipSetting.Id, -1);
                        changeDict.Add(uneuquipId, 1);
                        MessagingSystem.Instance.QueueMessage(new ChangeItemMsg(changeDict));
                    }
                    else
                    {
                        playerEquips[equipSetting.EType] = equipSetting;
                        desc += equipSetting.EquipDesc + "。";
                        // 通知背包道具，删除一个装备物品
                        changeDict.Add(equipSetting.Id, -1);
                        MessagingSystem.Instance.QueueMessage(new ChangeItemMsg(changeDict));
                    }
                    // 发送到消息界面
                    MessagingSystem.Instance.QueueMessage(new BottomDialogMessage(desc));
                    break;
                case EquipState.Unequip:
                    playerEquips.Remove(equipSetting.EType);
                    changeDict.Add(equipSetting.Id, 1);

                    MessagingSystem.Instance.QueueMessage(new ChangeItemMsg(changeDict));
                    MessagingSystem.Instance.QueueMessage(new BottomDialogMessage(equipSetting.UnequipDesc + "。"));
                    break;
            }
            return true;
        }
        #endregion
    }

    #region 消息类
    class RequestPlayerFightInfo:BaseMessage{}

    class RequestPlayerEquips: BaseMessage{}

    class ChangeEquipMsg: BaseMessage
    {
        public readonly EquipState equipState;
        public readonly string equipId;
        public readonly string equipName;
        public ChangeEquipMsg(EquipState es, string id, string name)
        {
            equipState = es;
            equipId = id;
            equipName = name;
        }
    }
    #endregion
    
}