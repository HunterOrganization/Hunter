
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Basic.ResourceMgr;
namespace Hunter.SettingMgr
{
    public interface INpcGoodsTypeSetting
    {
        string Id { get; }
        string CountRange { get; }
        int Zidan1 { get; }
        int Shuihu1 { get; }
        int Yeshoupi1 { get; }
        int Lieqiang1 { get; }
        int Lieqiang2 { get; }
        int Lieqiang3 { get; }
        int Danyaodai1 { get; }
        int Qiangtao1 { get; }
        int Yeguo1 { get; }
        int Binggan1 { get; }
        int Shengrou1 { get; }
        int Shiyongrou1 { get; }
        int Pijiu1 { get; }
        int Tangguo1 { get; }
        int Mucai1 { get; }
        int Huozhong1 { get; }
        int Xiangyan1 { get; }
        int Yeshougu1 { get; }
        int Yumao1 { get; }
        int Shuijing1 { get; }
        int Yachi1 { get; }
        int Shugen1 { get; }
        int Bolipian1 { get; }
    }

    public class NpcGoodsTypeSetting : INpcGoodsTypeSetting
    {
        string id;
        string countRange;
        int zidan1;
        int shuihu1;
        int yeshoupi1;
        int lieqiang1;
        int lieqiang2;
        int lieqiang3;
        int danyaodai1;
        int qiangtao1;
        int yeguo1;
        int binggan1;
        int shengrou1;
        int shiyongrou1;
        int pijiu1;
        int tangguo1;
        int mucai1;
        int huozhong1;
        int xiangyan1;
        int yeshougu1;
        int yumao1;
        int shuijing1;
        int yachi1;
        int shugen1;
        int bolipian1;
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

        public string CountRange
        {
            get
            {
                return countRange;
            }

            set
            {
                countRange = value;
            }
        }

        public int Zidan1
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

        public int Shuihu1
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

        public int Yeshoupi1
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

        public int Lieqiang1
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

        public int Lieqiang2
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

        public int Lieqiang3
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

        public int Danyaodai1
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

        public int Qiangtao1
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

        public int Yeguo1
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

        public int Binggan1
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

        public int Shengrou1
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

        public int Shiyongrou1
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

        public int Pijiu1
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

        public int Tangguo1
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

        public int Mucai1
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

        public int Huozhong1
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

        public int Xiangyan1
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

        public int Yeshougu1
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

        public int Yumao1
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

        public int Shuijing1
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

        public int Yachi1
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

        public int Shugen1
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

        public int Bolipian1
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

    public class NpcGoodsTypeSettingPro : SettingModel<NpcGoodsTypeSetting, INpcGoodsTypeSetting>
    {
        public NpcGoodsTypeSettingPro(IResourceMgr resMgr)
        {
            filePath = "Setting/NpcGoodsTypeSetting";
            resourceMgr = resMgr;
            LoadSetting("Id");
        }
    }
}