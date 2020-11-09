namespace SingletonPattern
{
    // Singleton generyczny z lock
    public class Singleton<T>
        where T : new()         // constraint
    {
        protected Singleton()
        {
        }

        private static T instance;

        private static object syncLock = new object();

        public static T Instance
        {
            get
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }

                return instance;
            }
        }
    }
}
