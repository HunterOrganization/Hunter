using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter.UI
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField]
        float layer = 1.0f;

        Button btn;
        void Start()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate () 
                {
                    btn.interactable = false;
                    Invoke("ResetBtn", layer);
                });
        }

        void ResetBtn()
        {
            btn.interactable = true;
        }
    }
}