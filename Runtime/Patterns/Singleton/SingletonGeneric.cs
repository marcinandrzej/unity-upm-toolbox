using System;

namespace MA.Patterns.Singleton
{
    public abstract class SingletonGeneric<SingletonInterfaceT, SingletonClassT>
        where SingletonClassT : class, SingletonInterfaceT
        where SingletonInterfaceT : class
    {
        private static SingletonInterfaceT _instance;

        public static SingletonInterfaceT Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Activator.CreateInstance(typeof(SingletonClassT), true) as SingletonInterfaceT;

                return _instance;
            }
        }
    }
}
