using Basic.ResourceMgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.SettingMgr
{
    public interface IFoodSetting
    {
        string Id { get; }
        float Life { get; }
        float Hot { get; }
        float Alone { get; }
        float Hunger { get; }
        string Describe { get; }

    }

    public class FoodSetting: IFoodSetting
    {
        string id;
        float life;
        float hot;
        float alone;
        float hunger;
        string describe;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public float Life
        {
            get
            {
                return life;
            }

            set
            {
                life = value;
            }
        }

        public float Hot
        {
            get
            {
                return hot;
            }

            set
            {
                hot = value;
            }
        }

        public float Alone
        {
            get
            {
                return alone;
            }

            set
            {
                alone = value;
            }
        }

        public float Hunger
        {
            get
            {
                return hunger;
            }

            set
            {
                hunger = value;
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
    }

    public class FoodSettingPro : SettingModel<FoodSetting, IFoodSetting>
    {
        public FoodSettingPro(IResourceMgr resMgr)
        {
            filePath = "Setting/FoodSetting";
            resourceMgr = resMgr;
            LoadSetting("Id");
        }
    }

}