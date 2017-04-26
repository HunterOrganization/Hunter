using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.UISystem;
using UnityEngine.UI;
using Hunter.TradeSystem;

namespace Hunter.UI
{
    public class GoodsListController : MonoBehaviour
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

            dataGrid.SetItemRender(item, typeof(GoodsItemConfig));
            dataGrid.useLoopItems = true;
        }

        
        public void SetData(List<GoodsUIInfo> itemDatas)
        {
            dataGrid.Data = itemDatas.ToArray();
        }
    }
    
}