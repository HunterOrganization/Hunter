using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hunter.UISystem;
using Hunter.TradeSystem;

namespace Hunter.UI
{
    public class BagItemConfig : ItemRender
    {
        Text itemName;
        Text itemCount;
        Button selectBtn;

        ItemUIInfo itemData;

        public override void Awake()
        {
            initUIControl();
        }

        void initUIControl()
        {
            // 获取UI控件
            itemName = transform.Find("ItemName").GetComponent<Text>();
            itemCount = transform.Find("ItemCount").GetComponent<Text>();
            selectBtn = transform.GetComponent<Button>();
        }

        protected override void OnSetData(object data)
        {
            m_renderData = data;
            itemData = data as ItemUIInfo;

            itemName.text = itemData.itemInfo.itemSetting.Name;
            itemCount.text = itemData.itemInfo.count != 0 ? string.Format("x {0}", itemData.itemInfo.count): "(已装备)";
            // 按钮事件
            selectBtn.onClick.AddListener(delegate () { this.OnClick(itemData.itemInfo); });
        }

        void OnClick(BagItemInfo setting)
        {
            itemData.callback(setting);
        }
    }
}