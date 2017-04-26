using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Hunter.GameCore;
using Hunter.UISystem;
using UnityEngine.UI;
using Hunter.Tools;

namespace Hunter.UI
{
    public class MessageDialog : MonoBehaviour
    {
        [SerializeField]
        int LimitMaxCount = 30;

        public RectTransform ContentControl;
        public RectTransform ItemControl;

        private RectTransform _trans;

        private RectTransform transContent;
        private ScrollRect scrollRect;
        private GameObject item;

        private DataGrid dataGrid;

        // 数据内容
        List<object> itemDatas = new List<object>();

        void Start()
        {
            // 消息监听
            MessagingSystem.Instance.AttachListener(typeof(BottomDialogMessage), HandleAddMsg);

            _trans = GetComponent<RectTransform>();

            transContent = ContentControl.GetComponent<RectTransform>();
            scrollRect = _trans.GetComponent<ScrollRect>();

            item = ItemControl.gameObject;

            dataGrid = gameObject.AddComponent<DataGrid>();

            dataGrid.SetItemRender(item, typeof(ItemConfig));
            dataGrid.useLoopItems = true;
        }

        /// <summary>
        /// 响应添加数据的消息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isSetBottom"></param>
        bool HandleAddMsg(BaseMessage bm)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
            }
            BottomDialogMessage bdMsg = bm as BottomDialogMessage;
            
            int index = 0;
            List<string> msgs = new List<string>();
            Lib.GetSubStringList(bdMsg.content, (int)(ItemControl.sizeDelta.x/8.7f), msgs);

            foreach (var msg in msgs)
            {
                itemDatas.Insert(index, msg);
                ++index;
            }
            // 删除超过LimitMaxCount数量的信息
            if (itemDatas.Count > LimitMaxCount)
                itemDatas.RemoveRange(LimitMaxCount - 1, itemDatas.Count - LimitMaxCount);

            dataGrid.Data = itemDatas.ToArray();
            return true;
        }

        void OnDestory()
        {
            if (MessagingSystem.IsAlive)
                MessagingSystem.Instance.DetachListener(typeof(BottomDialogMessage), HandleAddMsg);
        }

        /// <summary>
        /// 显示listview框
        /// </summary>
        /// <param name="datas">数据列表内容</param>
        /// <param name="isSetBottom">是否显示至最底部</param>
        public void Show(List<object> datas, bool isSetBottom = false)
        {
            this.itemDatas = datas;
            
            dataGrid.Data = datas.ToArray();

            dataGrid.ResetScrollPosition();
        }
    }

    // 消息类
    public class BottomDialogMessage : BaseMessage
    {
        public readonly string content;

        public BottomDialogMessage(string data)
        {
            this.content = data;
        }
    }
}