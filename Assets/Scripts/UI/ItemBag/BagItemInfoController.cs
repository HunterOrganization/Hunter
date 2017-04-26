using DG.Tweening;
using Hunter.EquipSystem;
using Hunter.GameCore;
using Hunter.HunterInfo;
using Hunter.TradeSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter.UI
{
    public class BagItemInfoController : MonoBehaviour
    {
        Text itemDesc;
        Button usedBtn;
        void Start()
        {
            itemDesc = transform.Find("ItemDesc").GetComponent<Text>();
            usedBtn = transform.Find("UsedBtn").GetComponent<Button>();
            usedBtn.gameObject.SetActive(false);
        }

        /// <summary>
        /// 点击背包中物品项时触发
        /// </summary>
        /// <param name="setting"></param>
        public void SetInfo(BagItemInfo setting)
        {
            itemDesc.text = setting.itemSetting.Describe;
            usedBtn.onClick.RemoveAllListeners();
            // 根据类型显示按钮 
            switch (setting.itemSetting.ItemType)
            {
                case "Equip":
                    usedBtn.gameObject.SetActive(true);
                    usedBtn.transform.Find("Text").GetComponent<Text>().text = setting.count == 0? "卸载":"装备";
                    // 发出装备或卸载道具的通知
                    usedBtn.onClick.AddListener(delegate (){
                        MessagingSystem.Instance.QueueMessage(new ChangeEquipMsg(setting.count == 0?EquipState.Unequip:EquipState.Equip, 
                            setting.itemSetting.ItemId, setting.itemSetting.Name));
                        MessagingSystem.Instance.QueueMessage(new RequestOpenBagMsg());
                        //usedBtn.gameObject.SetActive(false);
                        CloseInfoPanel();
                    });
                    break;

                case "Edible":
                    usedBtn.gameObject.SetActive(true);
                    usedBtn.transform.Find("Text").GetComponent<Text>().text = "使用";
                    // 发出使用道具的通知
                    usedBtn.onClick.AddListener(delegate () {
                        MessagingSystem.Instance.QueueMessage(new UsedItemMsg(setting.itemSetting.ItemId));
                        MessagingSystem.Instance.QueueMessage(new RequestOpenBagMsg());
                        //usedBtn.gameObject.SetActive(false);
                        CloseInfoPanel();
                    });
                    break;

                default:
                    usedBtn.gameObject.SetActive(false);
                    CloseInfoPanel();
                    break;
            }
            Tweener tweener = transform.DOScaleX(1, 0.3f);
        }

        void CloseInfoPanel()
        {
            Tweener tweener = transform.DOScaleX(0, 0.3f);
        }
        
    }
}