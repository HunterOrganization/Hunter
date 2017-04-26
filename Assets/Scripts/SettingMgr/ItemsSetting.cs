using Basic.ResourceMgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.SettingMgr
{
    public interface IItemsSetting
    {
        string ItemId { get; }
        string Name { get; }
        int Value { get; }
        string Describe { get; }
        float Weight { get; }
        string ItemType { get; }
    }

    public class ItemsSetting : IItemsSetting
    {
        string itemId;
        string name;
        int value;
        string describe;
        float weight;
        string itemType;
        public string ItemId
        {
            get
            {
                return itemId;
            }

            set
            {
                itemId = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
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

        public float Weight
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

        public string ItemType
        {
            get
            {
                return itemType;
            }

            set
            {
                itemType = value;
            }
        }
    }
    
    public class ItemsSettingPro : SettingModel<ItemsSetting, IItemsSetting>
    {
        public ItemsSettingPro(IResourceMgr resMgr)
        {
            filePath = "Setting/ItemsSetting";
            resourceMgr = resMgr;
            LoadSetting("ItemId");
        }
    }
}