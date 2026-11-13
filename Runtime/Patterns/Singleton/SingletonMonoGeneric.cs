using UnityEngine;

namespace MA.Patterns.Singleton
{
    public abstract class SingletonMonoGeneric<SingletonInterfaceT, SingletonClassT> : MonoBehaviour 
        where SingletonClassT : MonoBehaviour, SingletonInterfaceT
        where SingletonInterfaceT : class
    {
        public static SingletonInterfaceT Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this as SingletonInterfaceT)
            {
                Destroy(this);

                return;
            }

            Instance = this as SingletonInterfaceT;
        }

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
        }
    }
}
