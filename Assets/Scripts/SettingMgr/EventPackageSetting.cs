using Basic.ResourceMgr;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.SettingMgr
{
    public interface IEventPackageSetting
    {
        int EventID { get; }
        string Describe { get; }
        string EventType { get; }
        string EventOpt1 { get; }
        string EventOpt2 { get; }
        string EventOpt3 { get; }
        string DescOpt1 { get; }
        string DescOpt2 { get; }
        string DescOpt3 { get; }
        string HunterAI { get; }
        string TradeAI { get; }
        int TriggerDist { get; }
        int NextEvent { get; }
        string TaskItem { get; }
        string Trigger { get; }
        int Spoils { get; }
    }

    public class EventPackageSetting: IEventPackageSetting
    {
        #region 表格属性
        int eventID;
        string describe;
        string eventType;
        string eventOpt1;
        string eventOpt2;
        string eventOpt3;
        string descOpt1;
        string descOpt2;
        string descOpt3;
        string hunterAI;
        string tradeAI;
        int triggerDist;
        int nextEvent;
        string taskItem;
        string trigger;
        int spoils;

        public int EventID
        {
            get
            {
                return eventID;
            }

            set
            {
                eventID = value;
            }
        }

        public string Describe
        {
            get
            {
                return describe;
            }

            set
            {
                describe = value;
            }
        }

        public string EventType
        {
            get
            {
                return eventType;
            }

            set
            {
                eventType = value;
            }
        }

        public string EventOpt1
        {
            get
            {
                return eventOpt1;
            }

            set
            {
                eventOpt1 = value;
            }
        }

        public string EventOpt2
        {
            get
            {
                return eventOpt2;
            }

            set
            {
                eventOpt2 = value;
            }
        }

        public string EventOpt3
        {
            get
            {
                return eventOpt3;
            }

            set
            {
                eventOpt3 = value;
            }
        }

        public string DescOpt1
        {
            get
            {
                return descOpt1;
            }

            set
            {
                descOpt1 = value;
            }
        }

        public string DescOpt2
        {
            get
            {
                return descOpt2;
            }

            set
            {
                descOpt2 = value;
            }
        }

        public string DescOpt3
        {
            get
            {
                return descOpt3;
            }

            set
            {
                descOpt3 = value;
            }
        }

        public string HunterAI
        {
            get
            {
                return hunterAI;
            }

            set
            {
                hunterAI = value;
            }
        }

        public string TradeAI
        {
            get
            {
                return tradeAI;
            }

            set
            {
                tradeAI = value;
            }
        }

        public int NextEvent
        {
            get
            {
                return nextEvent;
            }

            set
            {
                nextEvent = value;
            }
        }

        public int TriggerDist
        {
            get
            {
                return triggerDist;
            }

            set
            {
                triggerDist = value;
            }
        }

        public string TaskItem
        {
            get
            {
                return taskItem;
            }

            set
            {
                taskItem = value;
            }
        }

        public string Trigger
        {
            get
            {
                return trigger;
            }

            set
            {
                trigger = value;
            }
        }

        public int Spoils
        {
            get
            {
                return spoils;
            }

            set
            {
                spoils = value;
            }
        }
        #endregion
    }
    
    public class EventPackageSettingPro : SettingModel<EventPackageSetting, IEventPackageSetting>
    {
        public EventPackageSettingPro(IResourceMgr resMgr)
        {
            filePath = "Setting/EventPackage";
            resourceMgr = resMgr;
            LoadSetting("EventID");
        }
    }
}