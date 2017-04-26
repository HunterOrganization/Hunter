using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.SettingMgr;

namespace Hunter.EventSystem
{
    /// <summary>
    /// 事件实体对象（若短时间内出现过多的事件 将导致前者被后者覆盖）
    /// </summary>
    public class EventAimPerfab: MonoBehaviour
    {
        private GameObject eventEntity;
        private IEventPackageSetting setting;
        private int slId;
        // 优化用缓存
        Vector3 newPosition;

        public IEventPackageSetting Setting
        {
            get
            {
                return setting;
            }
        }

        public int SlId
        {
            get
            {
                return slId;
            }

            set
            {
                slId = value;
            }
        }

        public EventAimPerfab(Transform trans)
        {
            eventEntity = (GameObject)Instantiate(Resources.Load("Perfabs/EventAim"));
            eventEntity.transform.parent = trans;
            eventEntity.transform.position = trans.position;
        }

        public void ResetEntity(IEventPackageSetting iSetting, int id)
        {
            setting = iSetting;
            SlId = id;
        }

        public void SetActive(bool isActive)
        {
            if (eventEntity == null)
            {
                Debug.Log("错误发生于EventAimPerfab/SetActive, 对象eventEntity为空");
                return;
            }
            eventEntity.SetActive(isActive);
        }

        public void UpdateDisc(float disc)
        {
            if (eventEntity == null)
            {
                Debug.Log("错误发生于EventAimPerfab/UpdateDisc, 对象eventEntity为空");
                return;
            }
            
            newPosition = eventEntity.transform.localPosition;
            newPosition.x = disc;
            eventEntity.transform.localPosition = newPosition;
        }
    }
}
