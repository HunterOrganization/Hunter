using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.GameCore;
using Hunter.TradeSystem;
using Hunter.SettingMgr;
using DG.Tweening;

namespace Hunter.UI
{
    public class BagPanelController : MonoBehaviour
    {
        BagListController itemList;
        BagItemInfoController itemInfo;

        #region 系统事件
        void Start()
        {
            itemList = transform.Find("ItemList").GetComponent<BagListController>();
            itemInfo = transform.Find("ItemInfo").GetComponent<BagItemInfoController>();

            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(OpenBagMsg), HandleOpenBag);
            // -------------------------------------------------------------------------------------------------- //
        }

        void OnDestory()
        {
            if (MessagingSystem.IsAlive)
            {
                MessagingSystem.Instance.DetachListener(typeof(OpenBagMsg), HandleOpenBag);
            }
        }
        #endregion

        public void OnClosePanelEvent()
        {
            Tweener tweener = transform.DOScaleX(0, 0.3f);
        }

        #region 消息处理事件
        /// <summary>
        /// 打开背包并设置数据
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        bool HandleOpenBag(BaseMessage bm)
        {
            OpenBagMsg msg = bm as OpenBagMsg;
            List<ItemUIInfo> items = new List<ItemUIInfo>();

            foreach(var item in msg.bagList)
            {
                ItemUIInfo info = new ItemUIInfo(item, itemInfo.SetInfo);
                items.Add(info);
            }

            itemList.SetData(items);

            Tweener tweener = transform.DOScaleX(1, 0.3f);
            return true;
        }
        #endregion
    }

    #region 消息类型
    class OpenBagMsg: BaseMessage
    {
        public readonly List<BagItemInfo> bagList;
        public OpenBagMsg(List<BagItemInfo> bl)
        {
            bagList = bl;
        }
    }
    #endregion
}


