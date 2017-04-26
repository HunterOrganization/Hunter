using Hunter.EventSystem;
using Hunter.GameCore;
using Hunter.HunterInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.UI
{
    public class SecenController : MonoBehaviour, IUpaderable
    {
        Material AfterBgMat;
        Material BackageMat;

        GameObject Hunger;

        float move = 0.0f;
        bool isStop = false;
        void Start()
        {
            // 注册更新事件
            GameLogic.Instance.RegisterUpdateableObject(this);
            if(transform.Find("Hunter") != null)
                Hunger = transform.Find("Hunter").gameObject;

            if(transform.Find("GameAfterBg") != null)
                AfterBgMat = transform.Find("GameAfterBg").GetComponent<Renderer>().material;

            if(transform.Find("GameBackBg") != null )
                BackageMat = transform.Find("GameBackBg").GetComponent<Renderer>().material;
            
            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(UpdateEventStateMsg), HandleUpdateEventState);
            // MessagingSystem.Instance.AttachListener(typeof(UpdateMoveSpeed), HandleUpdateSpeedPanel); // 速度暂时不考虑
            // -------------------------------------------------------------------------------------------------- //
            // 这里负责游戏开始与游戏结束时的场景转换
        }

        public void OnUpdate(float dt)
        {
            if (!isStop)
            {
                move += dt;
                move = move > 100 ? move - 100 : move;
                if(AfterBgMat != null)
                    AfterBgMat.SetFloat("_Dict", move);
                if(BackageMat != null)
                    BackageMat.SetFloat("_Dict", move);
                if(Hunger != null)
                    Hunger.transform.localPosition = new Vector3(Hunger.transform.localPosition.x, 
                        -1.3f + (Mathf.Sin(move*3)/10), Hunger.transform.localPosition.z);
            }
        }

        void OnDestory()
        {
            if (MessagingSystem.IsAlive)
            {
                MessagingSystem.Instance.DetachListener(typeof(UpdateEventStateMsg), HandleUpdateEventState);
            }
        }

        #region 消息处理事件
        bool HandleUpdateEventState(BaseMessage bm)
        {
            UpdateEventStateMsg msg = bm as UpdateEventStateMsg;

            switch (msg.eventState)
            {
                case EventState.EventBegin:
                    isStop = true;
                    break;
                case EventState.EventEnd:
                    isStop = false;
                    break;
            }
            return true;
        }
        #endregion
    }

}