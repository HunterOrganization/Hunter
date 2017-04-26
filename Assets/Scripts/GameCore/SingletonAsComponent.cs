using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.GameCore {
    public class SingletonAsComponent<T> : MonoBehaviour where T : 
        SingletonAsComponent<T> {
        private static T _instance;

        protected static SingletonAsComponent<T> _Instance
        {
            get
            {
                if (!_instance)
                {
                    T[] managers = GameObject.FindObjectOfType(typeof(T)) as T[];
                    if (managers != null)
                    {
                        if (managers.Length == 1)
                        {
                            _instance = managers[0];
                            return _instance;
                        }
                        else if (managers.Length > 1)
                        {
                            Debug.LogError("场景中已经拥有超过一个的"
                                + typeof(T).Name);
                            for (int i = 0; i < managers.Length; i++)
                            {
                                T manager = managers[i];
                                Destroy(manager.gameObject);
                            }
                        }
                    }
                    GameObject go = new GameObject(typeof(T).Name, typeof(T));

                    _instance = go.GetComponent<T>();

                    // DontDestroyOnLoad函数使得对象在场景之间持久
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
            set
            {
                _instance = value as T;
            }
        }

        // 添加对生命周期的管理
        private bool _alive = true;
        void OnDestroy() { _alive = false; }
        void OnApplicationQuit() { _alive = false; }
        public static bool IsAlive
        {
            get
            {
                if (_instance == null)
                    return false;
                return _instance._alive;
            }
        }
    }
}