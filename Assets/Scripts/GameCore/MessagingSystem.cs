using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.GameCore
{
    public class BaseMessage
    {
        public string name;
        public BaseMessage() { name = GetType().Name; }
    }

    public class MessagingSystem : SingletonAsComponent<MessagingSystem>
    {
        public static MessagingSystem Instance
        {
            get { return (MessagingSystem)_Instance; }
            set { _Instance = value; }
        }

        // 委托队列
        public delegate bool MessageHandlerDelegate(BaseMessage message);
        private Dictionary<string, List<MessageHandlerDelegate>> _listenerDict = 
            new Dictionary<string, List<MessageHandlerDelegate>>();

        /// <summary>
        /// 添加对指定(自定义消息)类型的监听
        /// </summary>
        /// <param name="type">自定义消息类型</param>
        /// <param name="handler">委托句柄</param>
        /// <returns></returns>
        public bool AttachListener(System.Type type, MessageHandlerDelegate handler)
        {
            if (type == null)
            {
                Debug.Log("使用了一个空的类型");
                return false;
            }

            string msgName = type.Name;

            if (!_listenerDict.ContainsKey(msgName))
            {
                _listenerDict.Add(msgName, new List<MessageHandlerDelegate>());
            }

            List<MessageHandlerDelegate> listenerList = _listenerDict[msgName];
            if (listenerList.Contains(handler))
            {
                return false;
            }
            listenerList.Add(handler);
            return true;
        }

        // 消息队列
        private Queue<BaseMessage> _messageQueue = new Queue<BaseMessage>();
        public bool QueueMessage(BaseMessage msg, float layer = 0.0f)
        {
            if (!_listenerDict.ContainsKey(msg.name)) return false;

            _messageQueue.Enqueue(msg);
            return true;
        }

        // 消息循环处理
        public float maxQueueProcessingTime = 0.2f;
        void Update()
        {
            float timer = 0.0f;
            while (_messageQueue.Count > 0)
            {
                if (maxQueueProcessingTime > 0.0f)
                {
                    if (timer > maxQueueProcessingTime)
                        return;
                }
                BaseMessage msg = _messageQueue.Dequeue();
                if (!TriggerMessage(msg))
                    Debug.Log("处理" + msg.name + "时错误");
                if (maxQueueProcessingTime > 0.0f)
                    timer += Time.deltaTime;
            }
        }
        
        public bool TriggerMessage(BaseMessage msg)
        {
            string msgName = msg.name;
            if (!_listenerDict.ContainsKey(msgName))
            {
                Debug.Log("不存在的消息类型，无法触发");
                return false;
            }

            List<MessageHandlerDelegate> listenerList = _listenerDict[msgName];

            for (int i = 0; i < listenerList.Count; i++)
            {
                if (!listenerList[i](msg))
                {
                    Debug.Log(string.Format("消息{0}处理失败", msgName));
                }
                    
            }
            return true;
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="type">(自定义)消息类型</param>
        /// <param name="handler">委托句柄</param>
        /// <returns></returns>
        public bool DetachListener(System.Type type, MessageHandlerDelegate handler)
        {
            if (type == null)
            {
                Debug.Log("无效的消息类型 in DetachListener");
                return false;
            }
            string msgName = type.Name;
            if (!_listenerDict.ContainsKey(msgName))
            {
                return false;
            }

            List<MessageHandlerDelegate> listenerList = _listenerDict[msgName];
            if (listenerList.Contains(handler))
                return false;

            listenerList.Remove(handler);
            return true;
        }

    }
}
