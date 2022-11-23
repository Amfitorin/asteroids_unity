using System;

namespace Core.Singleton
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = new T();
                _instance.Initialize();

                return _instance;
            }
        }

        public static bool Try(Action<T> callback)
        {
            if (_instance == null)
                return false;
            callback(_instance);
            return true;
        }

        public static void ReleaseInstance()
        {
            if (_instance != null)
                _instance.DoRelease();
            _instance = null;
        }

        protected abstract void Initialize();

        protected virtual void DoRelease()
        {
        }
    }
}