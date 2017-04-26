using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hunter.GameCore;
using Basic.ResourceMgr;
using Hunter.SettingMgr;
using Hunter.Tools;
using Hunter.TradeSystem;
using Hunter.UI;

namespace Hunter.EventSystem {
    enum EndType
    {
        CompleteEnd = -1,
        RandomEnd,
    }
    
    public class StoryLine
    {
        public IStoryLineSetting Setting; // 故事线设置
        public int EventId;     // 当前故事线ID
        public int Times = 1;   // 故事线连续出现的次数
    }
    
    public class EventManager : MonoBehaviour
    {
        #region 缓存数据
        Dictionary<int, IStoryLineSetting> slSetting = new Dictionary<int, IStoryLineSetting>();
        Dictionary<int, StoryLine> storyLines = new Dictionary<int, StoryLine>(); // 已触发故事线

        Dictionary<string, Dictionary<int, IEventPackageSetting>> allSetting = new Dictionary<string, Dictionary<int, IEventPackageSetting>>();

        List<int> Happeneds = new List<int>(); // 已触节点事件
        #endregion

        #region 系统事件
        void Start() {
            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(TriggerMsg), HandleGetEventMsg); // 获取下一个事件
            MessagingSystem.Instance.AttachListener(typeof(NextMsg), HandleNextMsg); // 触发下一个对话框
            // -------------------------------------------------------------------------------------------------- //
            LoadSetting();
        }


        void OnDestory()
        {
            if (MessagingSystem.IsAlive)
            {
                MessagingSystem.Instance.DetachListener(typeof(TriggerMsg), HandleGetEventMsg);
                MessagingSystem.Instance.DetachListener(typeof(NextMsg), HandleNextMsg);
            }
        }
        #endregion
        

        #region 消息处理事件
        bool HandleGetEventMsg(BaseMessage bm)
        {
            TriggerMsg em = bm as TriggerMsg;
            // 确认是否触发节点
            foreach(KeyValuePair<int, IEventPackageSetting> ke in allSetting["Key"])//keyEvents)
            {
                if (ke.Value.TriggerDist != 0 && em.distance >= ke.Value.TriggerDist &&
                    !Happeneds.Contains(ke.Key))
                {
                    SendEventToUI(em.distance, ke.Value);
                    Happeneds.Add(ke.Key);
                    return true;
                }
            }

            int slId = GetStoryLineId();

            IEventPackageSetting se = GetStoryEvent(slId);

            SendEventToUI(em.distance, se, slId);
            return true;
        }

        bool HandleNextMsg(BaseMessage bm)
        {
            NextMsg msg = bm as NextMsg;

            IEventPackageSetting se = msg.storyLineId < 0 ? allSetting["Side"][msg.eventId] : GetStoryEvent(msg.storyLineId);

            if(se == null)
            {
                Debug.Log(string.Format("错误发生于 EventManager/HandleNextMsg中，错误的事件ID:{0}", msg.eventId));
                return true;
            }

            // 确认是否触发新的事件
            if (se.Trigger != "")
            {
                AddStoryLink(int.Parse(se.Trigger));
            }

            MessagingSystem.Instance.QueueMessage(new EventUIMessage(se, msg.storyLineId));
            return true;
        }
        #endregion

        #region 逻辑处理
        /// <summary>
        /// 读取CSV表格内容
        /// </summary>
        void LoadSetting()
        {
            // 读取事件包数据
            IResourceMgr rm = new ResourceMgr();
            EventPackageSettingPro epPro = new EventPackageSettingPro(rm);
            Dictionary<string, EventPackageSetting>.KeyCollection epKeys = epPro.GetAllKey();

            foreach (var key in epKeys)
            {
                var item = epPro.GetItem(key);
                if (!allSetting.ContainsKey(item.EventType))
                    allSetting[item.EventType] = new Dictionary<int, IEventPackageSetting>();

                allSetting[item.EventType].Add(item.EventID, item);
            }

            // 读取故事线数据
            StoryLineSettingPro ssPro = new StoryLineSettingPro(rm);
            Dictionary<string, StoryLineSetting>.KeyCollection ssKeys = ssPro.GetAllKey();
            foreach (var key in ssKeys)
            {
                var item = ssPro.GetItem(key);
                slSetting.Add(item.StoryLineID, item);
            }
        }

        /// <summary>
        /// 根据权值随机选取一条故事线 (需要优化下算法)
        /// </summary>
        /// <returns>故事线</returns>
        int GetStoryLineId()
        {
            if (storyLines.Count <= 0)
            {
                Debug.Log("故事线列表为空，请确认问题");
                return -1;
            }
            if (storyLines.Count == 1)
                return storyLines.Keys.First();

            Dictionary<int, StoryLine>.KeyCollection keyColl = storyLines.Keys;

            int[] weightArray = new int[storyLines.Count];
            int index = 0;
            foreach (int slId in keyColl)
            {
                weightArray[index] = (int)storyLines[slId].Setting.Weight / (storyLines[slId].Times);

                storyLines[slId].Times = storyLines[slId].Times > 1 ? storyLines[slId].Times - 1 : 1;
                ++index;
            }

            int[] result = Lib.GetIndexByWeight(weightArray, 1);

            storyLines.Keys.CopyTo(weightArray, 0);

            storyLines[weightArray[result[0]]].Times += 1;

            return weightArray[result[0]];
        }

        /// <summary>
        /// 获取故事包 并更新故事线
        /// </summary>
        /// <param name="sl">故事线</param>
        /// <returns>事件包</returns>
        IEventPackageSetting GetStoryEvent(int slId)
        {
            if (slId < 0) return null;

            IEventPackageSetting se = null;
            StoryLine sl = storyLines[slId];

            foreach(var pair in allSetting)
            {
                if(pair.Value.ContainsKey(sl.EventId))
                    se = pair.Value[sl.EventId];
            }

            if(se == null)
            {
                // 错误的故事包ID 进行错误提示 结束故事线
                Debug.Log(string.Format("不存在的事件包ID{0}，默认返回提示事件", sl.EventId));
                return null;
            }

            // 根据获取的故事包 更新故事线
            if (se.NextEvent == (int)EndType.CompleteEnd) // 故事线最后一个故事，删除故事线
            {
                storyLines.Remove(sl.Setting.StoryLineID);
            }
            else if (se.NextEvent == (int)EndType.RandomEnd) // 随机故事线，需要再随机一个后续事件包
            {
                int idx = 0;
                int rValue = Random.Range(0, allSetting[se.EventType].Count);//randomEvents.Count);

                foreach (KeyValuePair<int, IEventPackageSetting> story in allSetting[se.EventType])
                {
                    if (rValue == idx)
                    {
                        sl.EventId = story.Key;
                        break;
                    }
                    idx++;
                }
            }
            else
            {
                sl.EventId = se.NextEvent;
                Debug.Log("更新故事线到" + sl.EventId);
            }
            Debug.Log("获得的故事ID是" + se.EventID);
            return se;
        }
        /// <summary>
        /// 添加新故事线（需要注意的是随机事件也是作为一个无限数量的故事线存在）
        /// </summary>
        /// <param name="slId">故事线ID</param>
        void AddStoryLink(int slId)
        {
            Debug.Log("添加了新的故事线" + slId);
            if (!storyLines.ContainsKey(slId) && slSetting.ContainsKey(slId))
            {
                StoryLine sl = new StoryLine();
                sl.Setting = slSetting[slId];
                sl.Times = 1;
                sl.EventId = sl.Setting.StartEvent;
                storyLines.Add(slId, sl);
            }
            else
            {
                Debug.Log(string.Format("故事线{0}已存在或配置表中无相应配置", slId));
            }
        }
        /// <summary>
        /// 通知EventController创建新的故事预设体 由预设体来触发事件UI界面
        /// </summary>
        /// <param name="dist"></param>
        /// <param name="setting"></param>
        void SendEventToUI(double dist, IEventPackageSetting setting, int slId = -1)
        {
            //Debug.Log(string.Format("当前是{0}", setting!=null?setting.Describe:"空事件"));
            MessagingSystem.Instance.QueueMessage(new CreateEventPoint(dist, setting, slId));

            // 确认是否触发新的事件
            if(setting != null && setting.Trigger != "")
            {
                //Debug.Log(string.Format("触发新的故事线{0}", setting.Describe));
                AddStoryLink(int.Parse(setting.Trigger));
            }
        }
        #endregion
    }

    #region 消息类型
    public class TriggerMsg : BaseMessage
    {
        public readonly double distance;
        public TriggerMsg(double t)
        {
            distance = t;
        }
    }

    public class NextMsg : BaseMessage
    {
        public readonly int eventId;
        public readonly int storyLineId;
        public NextMsg(int id, int slId)
        {
            eventId = id;
            storyLineId = slId;
        }
    }
    #endregion
}