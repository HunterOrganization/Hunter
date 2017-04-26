using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hunter.SettingMgr;
using Hunter.GameCore;
using Hunter.EventSystem;
using System;
using DG.Tweening;
using Hunter.Fight;
using Hunter.TradeSystem;
using Hunter.HunterInfo;

namespace Hunter.UI
{
    public class EventMainPanel : MonoBehaviour
    {
        [SerializeField]
        private Text describeText;

        [SerializeField]
        private Button[] optBoxs;

        #region 系统事件
        void Start()
        {
            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(EventUIMessage), HandleShowEevent);
            // -------------------------------------------------------------------------------------------------- //
        }

        void OnDestory()
        {
            if (MessagingSystem.IsAlive)
            {
                MessagingSystem.Instance.DetachListener(typeof(EventUIMessage), HandleShowEevent);
            }
        }
        #endregion

        #region 消息处理事件
        bool HandleShowEevent(BaseMessage bm)
        {
            EventUIMessage em = bm as EventUIMessage;
            describeText.text = em.eventSetting.Describe;
            Type settingType = em.eventSetting.GetType();

            describeText.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);

            int index = 1;
            foreach(var optBtn in optBoxs)
            {
                optBtn.gameObject.SetActive(true);
                var descOptInfo = settingType.GetProperty("DescOpt" + index);
                var eventOptInfo = settingType.GetProperty("EventOpt" + index);
                string desc = descOptInfo.GetValue(em.eventSetting, null) as string;
                if (desc != "")
                {
                    optBtn.GetComponent<Text>().text = (descOptInfo.GetValue(em.eventSetting, null) as string);
                    optBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                    optBtn.GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        this.OnOptClick(eventOptInfo.GetValue(em.eventSetting, null) as string, em.eventSetting, em.slId);
                    });
                }
                else
                {
                    optBtn.gameObject.SetActive(false);
                }

                ++index;
            }

            Tweener tweener = gameObject.transform.DOScaleX(1, 0.5f);
            return true;
        }
        #endregion

        #region 逻辑事件
        void OnOptClick(string config, IEventPackageSetting setting, int slId)
        {
            bool isClosePanel = true;
            switch (config)
            {
                case "Trade":  // 交易
                    Debug.Log(setting.TradeAI);
                    MessagingSystem.Instance.QueueMessage(new UpdateTradeStepMsg(TradeStep.BeginTrade, setting.TradeAI, setting.Spoils == 1));
                    break;

                case "Fight":  // 战斗
                    MessagingSystem.Instance.QueueMessage(new UpdateFightStepMsg(FightState.Begin, setting.HunterAI, setting.TradeAI));
                    break;

                case "Skip":   // 跳过
                    // 通知事件完毕
                    MessagingSystem.Instance.QueueMessage(new UpdateEventStateMsg(EventState.EventEnd));
                    break;

                case "Next":    // 下一个故事
                    MessagingSystem.Instance.QueueMessage(new NextMsg(setting.NextEvent, slId));
                    isClosePanel = false;
                    break;

                default:
                    Debug.Log("错误发生于EventMainPanel/OnOptClick:错误的事件包配置。");
                    break;
            }

            // 若有添加道具的配置
            if(setting.TaskItem != "")
            {
                string[] configs = setting.TaskItem.Split('|');

                if(configs.Length == 2)
                {
                    MessagingSystem.Instance.QueueMessage(new RequestAddItemMsg(int.Parse(configs[1]), configs[0]));
                }
                else
                {
                    Debug.Log("错误发生于 EventMainPanel/OnOptClick, 错误的任务道具配置" + setting.TaskItem);
                }
            }

            if (isClosePanel)
            {
                Tweener tweener = gameObject.transform.DOScaleX(0, 0.5f);
            }
        }
        #endregion
    }

    #region 消息类 
    public class EventUIMessage : BaseMessage
    {
        public readonly IEventPackageSetting eventSetting;
        public readonly int slId;
        public EventUIMessage(IEventPackageSetting setting, int id)
        {
            eventSetting = setting;
            slId = id;
        }
    }
    #endregion
}