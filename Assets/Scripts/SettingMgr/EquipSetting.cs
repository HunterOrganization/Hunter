using Basic.ResourceMgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.SettingMgr
{
    public interface IEquipSetting
    {
        string Id { get; }
        string EType { get; }
        int Attack { get; }
        int Defense { get; }
        string EquipDesc { get; }
        string UnequipDesc { get; }
    }
    public class EquipSetting: IEquipSetting
    {
        string id;
        string eType;
        int attack;
        int defense;
        string equipDesc;
        string unequipDesc;

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

        public string EType
        {
            get
            {
                return eType;
            }

            set
            {
                eType = value;
            }
        }

        public int Attack
        {
            get
            {
                return attack;
            }

            set
            {
                attack = value;
            }
        }

        public int Defense
        {
            get
            {
                return defense;
            }

            set
            {
                defense = value;
            }
        }

        public string EquipDesc
        {
            get
            {
                return equipDesc;
            }

            set
            {
                equipDesc = value;
            }
        }

        public string UnequipDesc
        {
            get
            {
                return unequipDesc;
            }

            set
            {
                unequipDesc = value;
            }
        }
    }

    public class EquipSettingPro : SettingModel<EquipSetting, IEquipSetting>
    {
        public EquipSettingPro(IResourceMgr resMgr)
        {
            filePath = "Setting/EquipSetting";
            resourceMgr = resMgr;
            LoadSetting("Id");
        }
    }
}