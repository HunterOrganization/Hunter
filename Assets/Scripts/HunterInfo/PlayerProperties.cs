using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hunter.HunterInfo
{
    public class PlayerProperties
    {
        float life;
        float hot;
        float hunger;
        float alone;
        float maxLife;
        float maxHot;
        float maxHunger;
        float maxAlone;

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

        public float MaxLife
        {
            get
            {
                return maxLife;
            }

            set
            {
                maxLife = value;
            }
        }

        public float MaxHot
        {
            get
            {
                return maxHot;
            }

            set
            {
                maxHot = value;
            }
        }

        public float MaxHunger
        {
            get
            {
                return maxHunger;
            }

            set
            {
                maxHunger = value;
            }
        }

        public float MaxAlone
        {
            get
            {
                return maxAlone;
            }

            set
            {
                maxAlone = value;
            }
        }
        
        /// <summary>
        /// 猎人属性配置
        /// </summary>
        /// <param name="l">生命值</param>
        /// <param name="h">热度</param>
        /// <param name="hg">饥饿</param>
        /// <param name="a">孤独</param>
        public PlayerProperties(float l, float h, float hg, float a, float ma)
        {
            MaxLife = life = l;
            MaxHot = hot = h;
            MaxHunger = hunger = hg;
            alone = a;
            MaxAlone = ma;
        }
    }
}