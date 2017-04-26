using UnityEngine;
using System.Collections;
using Hunter.UISystem;
using UnityEngine.UI;

namespace Hunter.UI
{
    public class ItemConfig : ItemRender
    {
        Text label;

        public override void Awake()
        {
            initUIControl();
        }

        void initUIControl()
        {
            label = transform.Find("Text").GetComponent<Text>();
        }
        
        protected override void OnSetData(object data)
        {
            m_renderData = data;
            string itemData = data as string;
            SetData(itemData);
        }

        private void SetData(string text)
        {
            label.text = text;
        }
    }
}