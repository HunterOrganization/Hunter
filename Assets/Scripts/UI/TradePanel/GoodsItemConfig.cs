using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hunter.UISystem;
using Hunter.TradeSystem;

namespace Hunter.UI
{
    public class GoodsItemConfig : ItemRender
    {
        Text goodsName;
        Button leftBtn;
        Button rightBtn;
        Text tradeNum;

        GoodsUIInfo itemData;

        public override void Awake()
        {
            initUIControl();
        }

        void initUIControl()
        {
            // 获取UI控件
            goodsName = transform.Find("GoodsName").GetComponent<Text>();
            tradeNum = transform.Find("TradeNum").GetComponent<Text>();
            leftBtn = transform.Find("LeftBtn").GetComponent<Button>();
            rightBtn = transform.Find("RightBtn").GetComponent<Button>();

            // 按钮事件
            leftBtn.onClick.AddListener(delegate () { this.OnClick(-1); });
            rightBtn.onClick.AddListener(delegate () { this.OnClick(1); });
        }

        protected override void OnSetData(object data)
        {
            m_renderData = data;
            itemData = data as GoodsUIInfo;

            goodsName.text = string.Format("{0} x{1}", itemData.gInfo.name, itemData.gInfo.count);
            tradeNum.text = string.Format("给出 x{0}", itemData.gInfo.giveCount);
        }
        
        void OnClick(int num)
        {
            int sum = itemData.gInfo.giveCount + num;

            if (sum > itemData.gInfo.count || sum < 0)
                return;

            itemData.gInfo.giveCount = sum; // 直接修改了引用
            tradeNum.text = string.Format("给出 x{0}", sum);

            itemData.callback();
        }
    }
}