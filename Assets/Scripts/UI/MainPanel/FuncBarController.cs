using Hunter.GameCore;
using Hunter.HunterInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter.UI
{
    public class FuncBarController : MonoBehaviour
    {
        Button myBagBtn;
        void Start()
        {
            myBagBtn = transform.Find("MyBag").GetComponent<Button>();

            myBagBtn.onClick.AddListener(delegate()
            {
                MessagingSystem.Instance.QueueMessage(new RequestOpenBagMsg());
            });
        }
    }
}