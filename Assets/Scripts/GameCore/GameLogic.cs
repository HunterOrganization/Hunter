using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.GameCore
{
    public interface IUpaderable
    {
        void OnUpdate(float dt);
    }

    public class GameLogic : SingletonAsComponent<GameLogic> {
        public static GameLogic Instance
        {
            get { return (GameLogic)_Instance; }
            set { _Instance = value; }
        }

        List<IUpaderable> _updateables = new List<IUpaderable>();
        public void RegisterUpdateableObject(IUpaderable obj)
        {
            if (!_updateables.Contains(obj))
            {
                _updateables.Add(obj);
            }
        }

        public void DeregisterUpdateableObject(IUpaderable obj)
        {
            if (_updateables.Contains(obj))
            {
                _updateables.Remove(obj);
            }
        }

        void Update()
        {
            float dt = Time.deltaTime;
            for (int i = 0; i < _updateables.Count; i++)
            {
                _updateables[i].OnUpdate(dt);
            }
        }

    }

}