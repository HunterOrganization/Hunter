using Basic.ResourceMgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.SettingMgr
{
    public interface IEnemySetting
    {
        string Id { get; }
        string Name { get; }
        int Attack { get; }
        int Defense { get; }
        string Describe { get; }
    }

    public class EnemySetting: IEnemySetting
    {
        string id;
        string name;
        int attack;
        int defense;
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

    public class EnemySettingPro : SettingModel<EnemySetting, IEnemySetting>
    {
        public EnemySettingPro(IResourceMgr resMgr)
        {
            filePath = "Setting/EnemySetting";
            resourceMgr = resMgr;
            LoadSetting("Id");
        }
    }
}