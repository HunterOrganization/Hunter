using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Basic.ResourceMgr;

namespace Hunter.SettingMgr
{
    public interface INpcGoodsNumSetting
    {
        string Id { get; }
        string Zidan1 { get; }
        string Shuihu1 { get; }
        string Yeshoupi1 { get; }
        string Lieqiang1 { get; }
        string Lieqiang2 { get; }
        string Lieqiang3 { get; }
        string Danyaodai1 { get; }
        string Qiangtao1 { get; }
        string Yeguo1 { get; }
        string Binggan1 { get; }
        string Shengrou1 { get; }
        string Shiyongrou1 { get; }
        string Pijiu1 { get; }
        string Tangguo1 { get; }
        string Mucai1 { get; }
        string Huozhong1 { get; }
        string Xiangyan1 { get; }
        string Yeshougu1 { get; }
        string Yumao1 { get; }
        string Shuijing1 { get; }
        string Yachi1 { get; }
        string Shugen1 { get; }
        string Bolipian1 { get; }
    }

    public class NpcGoodsNumSetting : INpcGoodsNumSetting
    {
        string id;
        string zidan1;
        string shuihu1;
        string yeshoupi1;
        string lieqiang1;
        string lieqiang2;
        string lieqiang3;
        string danyaodai1;
        string qiangtao1;
        string yeguo1;
        string binggan1;
        string shengrou1;
        string shiyongrou1;
        string pijiu1;
        string tangguo1;
        string mucai1;
        string huozhong1;
        string xiangyan1;
        string yeshougu1;
        string yumao1;
        string shuijing1;
        string yachi1;
        string shugen1;
        string bolipian1;
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

        public string Zidan1
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

        public string Shuihu1
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

        public string Yeshoupi1
        {
            get
            {
                return Yeshoupi11;
            }

            set
            {
                Yeshoupi11 = value;
            }
        }

        public string Yeshoupi11
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

        public string Lieqiang1
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

        public string Lieqiang2
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

        public string Lieqiang3
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

        public string Danyaodai1
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

        public string Qiangtao1
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

        public string Yeguo1
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

        public string Binggan1
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

        public string Shengrou1
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

        public string Shiyongrou1
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

        public string Pijiu1
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

        public string Tangguo1
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

        public string Mucai1
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

        public string Huozhong1
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

        public string Xiangyan1
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

        public string Yeshougu1
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

        public string Yumao1
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

        public string Shuijing1
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

        public string Yachi1
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

        public string Shugen1
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

        public string Bolipian1
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
    
    public class NpcGoodsNumSettingPro : SettingModel<NpcGoodsNumSetting, INpcGoodsNumSetting>
    {
        public NpcGoodsNumSettingPro(IResourceMgr resMgr)
        {
            filePath = "Setting/NpcGoodsNumSetting";
            resourceMgr = resMgr;
            LoadSetting("Id");
        }
    }
}