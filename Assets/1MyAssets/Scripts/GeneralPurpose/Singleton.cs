using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.GeneralPurpose
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component 
    {
        private static T _instance;
        public static T GetInstance()
        {
            if (_instance.IsDestroyed())
                _instance = null;

            _instance ??= FindObjectOfType<T>();
            _instance ??= new GameObject(typeof(T).Name).AddComponent<T>();

            return _instance;
        }
    }
}