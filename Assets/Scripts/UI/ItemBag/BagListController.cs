using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.UISystem;
using UnityEngine.UI;
using Hunter.TradeSystem;
using Hunter.HunterInfo;
using Hunter.SettingMgr;

namespace Hunter.UI
{
    public class BagListController : MonoBehaviour
    {
        public RectTransform ContentControl;
        public RectTransform ItemControl;

        private RectTransform _trans;

        private RectTransform transContent;
        private ScrollRect scrollRect;
        private GameObject item;

        private DataGrid dataGrid;

        void Start()
        {
            _trans = GetComponent<RectTransform>();

            transContent = ContentControl.GetComponent<RectTransform>();
            scrollRect = _trans.GetComponent<ScrollRect>();

            item = ItemControl.gameObject;

            dataGrid = gameObject.AddComponent<DataGrid>();

            dataGrid.SetItemRender(item, typeof(BagItemConfig));
            dataGrid.useLoopItems = true;
        }
        
        public void SetData(List<ItemUIInfo> itemDatas)
        {
            dataGrid.Data = itemDatas.ToArray();
        }
    }


    public class ItemUIInfo
    {
        public delegate void OnClickEvent(BagItemInfo id);
        public OnClickEvent callback;
        public readonly BagItemInfo itemInfo;
        public ItemUIInfo(BagItemInfo bi, OnClickEvent cb)
        {
            itemInfo = bi;
            callback = cb;
        }
    }
}