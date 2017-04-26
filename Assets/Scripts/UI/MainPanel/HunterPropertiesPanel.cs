using Hunter.GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hunter.HunterInfo;
using Hunter.EventSystem;

namespace Hunter.UI {
    public class HunterPropertiesPanel : MonoBehaviour, IUpaderable
    {
        float moveSpeed = 0;
        double distation = 0;
        bool isStop = false;
        // 缓存UI对象
        Dictionary<string, Text> UIList;


        #region 系统事件
        void Start() {
            // 初始化UI控件
            UIList = new Dictionary<string, Text>();
            UIList.Add("HunterName", transform.Find("HunterName").GetComponent<Text>());
            UIList.Add("Life", transform.Find("Life").GetComponent<Text>());
            UIList.Add("Hot", transform.Find("Hot").GetComponent<Text>());
            UIList.Add("Lonely", transform.Find("Lonely").GetComponent<Text>());
            UIList.Add("Speed", transform.Find("Speed").GetComponent<Text>());
            UIList.Add("Distance", transform.Find("Distance").GetComponent<Text>());
            UIList.Add("Hunger", transform.Find("Hunger").GetComponent<Text>());
            
            // 注册更新事件
            GameLogic.Instance.RegisterUpdateableObject(this);

            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(UpdateInfoMsg), HandleUpdateHunterInfo);
            MessagingSystem.Instance.AttachListener(typeof(UpdateMoveSpeed), HandleUpdateSpeedPanel); // 速度刷新
            MessagingSystem.Instance.AttachListener(typeof(UpdateEventStateMsg), HandleUpdateEventState);
            // -------------------------------------------------------------------------------------------------- //
        }

        public void OnUpdate(float dt)
        {
            if (!isStop && moveSpeed > 0)
            {
                distation += dt * moveSpeed;
                UIList["Distance"].text = string.Format("{0:0.0}m", distation);
            }
        }

        void OnDestory()
        {

            if (MessagingSystem.IsAlive)
            {
                MessagingSystem.Instance.DetachListener(typeof(UpdateInfoMsg), HandleUpdateHunterInfo);
                MessagingSystem.Instance.DetachListener(typeof(UpdateMoveSpeed), HandleUpdateSpeedPanel);
                MessagingSystem.Instance.DetachListener(typeof(UpdateEventStateMsg), HandleUpdateEventState);
            }
        }
        #endregion

        #region 消息处理事件
        bool HandleUpdateHunterInfo(BaseMessage bm)
        {
            UpdateInfoMsg msg = bm as UpdateInfoMsg;

            UIList["Life"].text = string.Format("{0:0.0}/{1}", msg.hunterProperties.Life, msg.hunterProperties.MaxLife);
            UIList["Hot"].text = string.Format("{0:0.0}/{1}", msg.hunterProperties.Hot, msg.hunterProperties.MaxHot);
            UIList["Lonely"].text = string.Format("{0:0.0}/{1}", msg.hunterProperties.Alone, msg.hunterProperties.MaxAlone);
            UIList["Hunger"].text = string.Format("{0:0.0}/{1}", msg.hunterProperties.Hunger, msg.hunterProperties.MaxHunger);
            return true;
        }
        bool HandleUpdateSpeedPanel(BaseMessage bm)
        {
            UpdateMoveSpeed msg = bm as UpdateMoveSpeed;
            moveSpeed = msg.speed;
            UIList["Speed"].text = string.Format("{0:0.0}m/s", moveSpeed);
            return true;
        }

        bool HandleUpdateEventState(BaseMessage bm)
        {
            UpdateEventStateMsg msg = bm as UpdateEventStateMsg;

            switch (msg.eventState)
            {
                case EventState.EventBegin:
                    isStop = true;
                    distation = msg.gameDist != 0 ? msg.gameDist : distation;
                    break;
                case EventState.EventEnd:
                    isStop = false;
                    break;
            }

            return true;
        }
        #endregion
    }


    #region 消息类
    public class UpdateInfoMsg: BaseMessage
    {
        public readonly PlayerProperties hunterProperties;
        public UpdateInfoMsg(PlayerProperties hp)
        {
            hunterProperties = hp;
        }
    }
    #endregion
}