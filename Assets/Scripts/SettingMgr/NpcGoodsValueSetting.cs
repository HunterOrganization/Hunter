using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Basic.ResourceMgr;

namespace Hunter.SettingMgr
{
    public interface INpcGoodsValueSetting
    {
        int Id { get; }
        float Zidan1 { get; }
        float Shuihu1 { get; }
        float Yeshoupi1 { get; }
        float Lieqiang1 { get; }
        float Lieqiang2 { get; }
        float Lieqiang3 { get; }
        float Danyaodai1 { get; }
        float Qiangtao1 { get; }
        float Yeguo1 { get; }
        float Binggan1 { get; }
        float Shengrou1 { get; }
        float Shiyongrou1 { get; }
        float Pijiu1 { get; }
        float Tangguo1 { get; }
        float Mucai1 { get; }
        float Huozhong1 { get; }
        float Xiangyan1 { get; }
        float Yeshougu1 { get; }
        float Yumao1 { get; }
        float Shuijing1 { get; }
        float Yachi1 { get; }
        float Shugen1 { get; }
        float Bolipian1 { get; }
    }

    public class NpcGoodsValueSetting : INpcGoodsValueSetting
    {
        int id;
        float zidan1;
        float shuihu1;
        float yeshoupi1;
        float lieqiang1;
        float lieqiang2;
        float lieqiang3;
        float danyaodai1;
        float qiangtao1;
        float yeguo1;
        float binggan1;
        float shengrou1;
        float shiyongrou1;
        float pijiu1;
        float tangguo1;
        float mucai1;
        float huozhong1;
        float xiangyan1;
        float yeshougu1;
        float yumao1;
        float shuijing1;
        float yachi1;
        float shugen1;
        float bolipian1;

        public int Id
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

        public float Zidan1
        {
            get
            {
                return zidan1;
            }

            set
            {
                zidan1 = value;
            }
        }

        public float Shuihu1
        {
            get
            {
                return shuihu1;
            }

            set
            {
                shuihu1 = value;
            }
        }

        public float Yeshoupi1
        {
            get
            {
                return yeshoupi1;
            }

            set
            {
                yeshoupi1 = value;
            }
        }


        public float Lieqiang1
        {
            get
            {
                return lieqiang1;
            }

            set
            {
                lieqiang1 = value;
            }
        }

        public float Lieqiang2
        {
            get
            {
                return lieqiang2;
            }

            set
            {
                lieqiang2 = value;
            }
        }

        public float Lieqiang3
        {
            get
            {
                return lieqiang3;
            }

            set
            {
                lieqiang3 = value;
            }
        }

        public float Danyaodai1
        {
            get
            {
                return danyaodai1;
            }

            set
            {
                danyaodai1 = value;
            }
        }

        public float Qiangtao1
        {
            get
            {
                return qiangtao1;
            }

            set
            {
                qiangtao1 = value;
            }
        }

        public float Yeguo1
        {
            get
            {
                return yeguo1;
            }

            set
            {
                yeguo1 = value;
            }
        }

        public float Binggan1
        {
            get
            {
                return binggan1;
            }

            set
            {
                binggan1 = value;
            }
        }

        public float Shengrou1
        {
            get
            {
                return shengrou1;
            }

            set
            {
                shengrou1 = value;
            }
        }

        public float Shiyongrou1
        {
            get
            {
                return shiyongrou1;
            }

            set
            {
                shiyongrou1 = value;
            }
        }

        public float Pijiu1
        {
            get
            {
                return pijiu1;
            }

            set
            {
                pijiu1 = value;
            }
        }

        public float Tangguo1
        {
            get
            {
                return tangguo1;
            }

            set
            {
                tangguo1 = value;
            }
        }

        public float Mucai1
        {
            get
            {
                return mucai1;
            }

            set
            {
                mucai1 = value;
            }
        }

        public float Huozhong1
        {
            get
            {
                return huozhong1;
            }

            set
            {
                huozhong1 = value;
            }
        }

        public float Xiangyan1
        {
            get
            {
                return xiangyan1;
            }

            set
            {
                xiangyan1 = value;
            }
        }

        public float Yeshougu1
        {
            get
            {
                return yeshougu1;
            }

            set
            {
                yeshougu1 = value;
            }
        }

        public float Yumao1
        {
            get
            {
                return yumao1;
            }

            set
            {
                yumao1 = value;
            }
        }

        public float Shuijing1
        {
            get
            {
                return shuijing1;
            }

            set
            {
                shuijing1 = value;
            }
        }

        public float Yachi1
        {
            get
            {
                return yachi1;
            }

            set
            {
                yachi1 = value;
            }
        }

        public float Shugen1
        {
            get
            {
                return shugen1;
            }

            set
            {
                shugen1 = value;
            }
        }

        public float Bolipian1
        {
            get
            {
                return bolipian1;
            }

            set
            {
                bolipian1 = value;
            }
        }
    }

    public class NpcGoodsValueSettingPro : SettingModel<NpcGoodsValueSetting, INpcGoodsValueSetting>
    {
        public NpcGoodsValueSettingPro(IResourceMgr resMgr)
        {
            filePath = "Setting/NpcGoodsValueSetting";
            resourceMgr = resMgr;
            LoadSetting("Id");
        }
    }
}