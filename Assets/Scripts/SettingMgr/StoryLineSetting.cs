using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Basic.ResourceMgr;

namespace Hunter.SettingMgr
{
    public interface IStoryLineSetting
    {
        int StoryLineID { get; }
        int StartEvent { get; }
        int Weight { get; }
    }
    public class StoryLineSetting : IStoryLineSetting
    {
        int storyLineID;
        int startEvent;
        int weight;

        public int StoryLineID
        {
            get
            {
                return storyLineID;
            }

            set
            {
                storyLineID = value;
            }
        }

        public int StartEvent
        {
            get
            {
                return startEvent;
            }

            set
            {
                startEvent = value;
            }
        }

        public int Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }
    }
    public class StoryLineSettingPro : SettingModel<StoryLineSetting, IStoryLineSetting>
    {
        public StoryLineSettingPro(IResourceMgr resMgr)
        {
            filePath = "Setting/StoryLineSetting";
            resourceMgr = resMgr;
            LoadSetting("StroyLineID");
        }
    }
}