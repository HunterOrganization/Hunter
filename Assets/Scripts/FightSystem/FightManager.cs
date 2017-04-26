using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.GameCore;
using Hunter.EquipSystem;
using Hunter.HunterInfo;
using Hunter.Tools;
using Hunter.UI;
using Hunter.TradeSystem;
using Hunter.SettingMgr;
using Basic.ResourceMgr;

namespace Hunter.Fight
{
    enum FightState
    {
        Begin,
        GetPlayerInfo,
        End,
    }

    public class FightManager : MonoBehaviour
    {
        // 缓存数据
        EnemySettingPro enemySettingPro;

        // 依旧临时存储的上下文
        string fightArgs = "";
        string tradeArgs = "";

        #region 系统事件
        void Start()
        {
            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(UpdateFightStepMsg), HandleUpdateFightStep);
            // -------------------------------------------------------------------------------------------------- //
            // 读取表格数据
            ResourceMgr rm = new ResourceMgr();
            enemySettingPro = new EnemySettingPro(rm);
        }
        
        void OnDectory()
        {
            MessagingSystem.Instance.DetachListener(typeof(UpdateFightStepMsg), HandleUpdateFightStep);
        }
        #endregion

        #region 消息处理事件
        bool HandleUpdateFightStep(BaseMessage bm)
        {
            UpdateFightStepMsg msg = bm as UpdateFightStepMsg;

            switch (msg.state)
            {
                case FightState.Begin:
                    fightArgs = msg.fightArgs;
                    tradeArgs = msg.tradeArgs;
                    // 获取玩家战斗数值
                    MessagingSystem.Instance.QueueMessage(new RequestPlayerFightInfo());
                    break;
                case FightState.GetPlayerInfo:
                    // 获取NPC战斗力 数据（生命值和子弹数量）
                    List<int> args = Lib.GetIntInString(fightArgs);

                    if(args.Count != 2)
                    {
                        Debug.Log("错误发生于 FightManager/HandleUpdateFightStep, 敌人配置错误~");
                        return false;
                    }
                    IEnemySetting es = enemySettingPro.GetItem(args[0].ToString());

                    if(es == null)
                    {
                        Debug.Log(string.Format("错误发生于 FightManager/HandleUpdateFightStep, 错误的敌人ID:{0}~", args[0]));
                        return false;
                    }

                    // 计算战斗结果
                    float fightResult = (msg.playerAttack - es.Attack * args[1] / 100) * 4 + (msg.playerDefense - es.Defense * args[1] / 100) * 5;
                    
                    if(fightResult < 0)
                    {
                        // 游戏结束
                        Debug.Log("游戏结束！");
                        MessagingSystem.Instance.QueueMessage(new GameOver());
                    }
                    else
                    {
                        // 通知消耗
                        int ammoCun = es.Defense * 2;
                        int lifeCun = (int)(es.Attack * (1 - msg.playerDefense * 0.06f / (1 + msg.playerDefense)));
                        MessagingSystem.Instance.QueueMessage(new FightConsumeMsg(ammoCun, lifeCun));
                        MessagingSystem.Instance.QueueMessage(new BottomDialogMessage(string.Format(es.Describe, lifeCun, ammoCun)));
                        // 进行交易 这是获取战利品
                        MessagingSystem.Instance.QueueMessage(new UpdateTradeStepMsg(TradeStep.BeginTrade, tradeArgs, true));
                    }
                    break;
                case FightState.End:

                    break;
            }
        
            return true;
        }
        #endregion

        #region 逻辑处理
        #endregion
    }
    #region 消息类
    class UpdateFightStepMsg: BaseMessage
    {
        public readonly FightState state;
        public readonly string fightArgs = "";
        public readonly string tradeArgs = "";
        public readonly int playerAttack = 0;
        public readonly int playerDefense = 0;
        public UpdateFightStepMsg(FightState fs, string fArgs, string tArgs)
        {
            state = fs;
            fightArgs = fArgs;
            tradeArgs = tArgs;
        }

        public UpdateFightStepMsg(FightState fs, int pa, int pd)
        {
            state = fs;
            playerAttack = pa;
            playerDefense = pd;
        }
    }

    class FightConsumeMsg: BaseMessage
    {
        public readonly int ammo;
        public readonly int life;
        
        public FightConsumeMsg(int a, int l)
        {
            ammo = a;
            life = l;
        }
    }
    #endregion

    // 配置类
    class EnemySetting
    {
        public int Id = 1;
        public string Name = "村民";
        public int Attack = 50;
        public int Defense = 10;
        public string FightDescribe = "经过一番艰辛的战斗，你终于打死那个村民，但是你损失了{0}点生命，消耗了{1}颗子弹";
    }
}