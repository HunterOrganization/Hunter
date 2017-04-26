using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.GameCore;
using Hunter.UI;
using Hunter.SettingMgr;

using System.Text.RegularExpressions;
using Hunter.HunterInfo;

namespace Hunter.EventSystem
{
    enum EventState
    {
        EventBegin,
        EventEnd
    };
    
    // 只负责对事件时间的抉择 通知EventManager触发事件的时间到了 但是不管触发的具体事件是什么
    // 负责生成事件实体
    public class EventsController : MonoBehaviour, IUpaderable
    {
        [SerializeField]
        float updateStateDist = 10f; // 刷新猎人属性的距离

        float hunterSpeed = 0; // 猎人移动速度
        bool isStop = false;

        // 距离相关
        double gameDist = 0.0f;
        double nextPoint = 0.0f;


        // 缓存数据
        EventAimPerfab eventAimList; // 事件对象列表 目前都只有一个事件
        EventAimPerfab eventAim; // 唯一的事件实体对象 复用
        double previousDist = 0; // 上一次更新状态的距离

        #region 系统事件
        void Start()
        {
            eventAim = new EventAimPerfab(transform);
            eventAim.SetActive(false);
            // 注册更新事件
            GameLogic.Instance.RegisterUpdateableObject(this);

            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(CreateEventPoint), HandleCreateEvent);
            MessagingSystem.Instance.AttachListener(typeof(UpdateMoveSpeed), HandleUpdateSpeed);
            MessagingSystem.Instance.AttachListener(typeof(UpdateEventStateMsg), HandleUpdateEventState);
            // -------------------------------------------------------------------------------------------------- //
        }

        public void OnUpdate(float dt)
        {

            if (hunterSpeed != 0 && !isStop)
            {
                UpdateEventPoint(); // 更新事件触发点
                UpdateHunterState(); // 更新猎人属性状态
                gameDist += dt* hunterSpeed;
            }
        }

        void OnDestory()
        {
            if (GameLogic.IsAlive)
                GameLogic.Instance.DeregisterUpdateableObject(this);

            if (MessagingSystem.IsAlive)
            {
                MessagingSystem.Instance.DetachListener(typeof(CreateEventPoint), HandleCreateEvent);
                MessagingSystem.Instance.DetachListener(typeof(UpdateMoveSpeed), HandleUpdateSpeed);
                MessagingSystem.Instance.DetachListener(typeof(UpdateEventStateMsg), HandleUpdateEventState);
            }
        }
        #endregion

        #region 逻辑处理
        void UpdateHunterState()
        {
            if(gameDist - previousDist > updateStateDist)
            {
                // 通知猎人属性管理 更新猎人数值
                MessagingSystem.Instance.QueueMessage(new UpdateHunterDist((float)(gameDist - previousDist), gameDist));
                previousDist = gameDist;
            }
        }
        /// <summary>
        /// 若当前事件已经达到触发条件 则触发当前事件并生成下一个点 否则更新事件对象
        /// </summary>
        void UpdateEventPoint()
        {
            if (nextPoint <= gameDist) { // 当前事件触发
                eventAim.SetActive(false);

                if (eventAim.Setting != null)
                {
                    isStop = true;
                    MessagingSystem.Instance.QueueMessage(new UpdateEventStateMsg(EventState.EventBegin, gameDist)); // 通知事件被触发
                    MessagingSystem.Instance.QueueMessage(new EventUIMessage(eventAim.Setting, eventAim.SlId)); // 通知UI出现事件窗口
                }
                // 首次触发
                if (gameDist == 0){
                    nextPoint = gameDist + Random.Range(1f, 2f);
                    MessagingSystem.Instance.QueueMessage(new TriggerMsg(nextPoint));
                }
            }
            else
            {
                eventAim.UpdateDisc((float)(nextPoint - gameDist));
            }
        }

        #endregion

        #region 消息事件
        bool HandleCreateEvent(BaseMessage bm)
        {
            CreateEventPoint cep = bm as CreateEventPoint;
            // 重置事件对象
            eventAim.ResetEntity(cep.setting, cep.slId);
            eventAim.SetActive(cep.setting != null);
            return true;
        }

        bool HandleUpdateSpeed(BaseMessage bm)
        {
            UpdateMoveSpeed msg = bm as UpdateMoveSpeed;
            hunterSpeed = msg.speed;
            return true;
        }

        bool HandleUpdateEventState(BaseMessage bm)
        {
            UpdateEventStateMsg msg = bm as UpdateEventStateMsg;

            if (msg.eventState == EventState.EventEnd)
            {
                nextPoint = gameDist + Random.Range(1f, 2f);
                MessagingSystem.Instance.QueueMessage(new TriggerMsg(nextPoint));
                isStop = false;
            }

            return true;
        }
        #endregion
    }

    #region 消息类
    class CreateEventPoint: BaseMessage
    {
        public readonly double distance;
        public readonly IEventPackageSetting setting;
        public readonly int slId;

        public CreateEventPoint(double dist, IEventPackageSetting setting, int id)
        {
            distance = dist;
            this.setting = setting;
            slId = id;
        }
    }
    class UpdateHunterDist: BaseMessage
    {
        public readonly float updateDist;
        public readonly double currDist;
        public UpdateHunterDist(float dist, double cd)
        {
            updateDist = dist;
            currDist = cd;
        }
    }
    class UpdateEventStateMsg: BaseMessage
    {
        public readonly double gameDist;
        public readonly EventState eventState;
        public UpdateEventStateMsg(EventState es, double gd = 0)
        {
            gameDist = gd;
            eventState = es;
        }
    }
    #endregion
}