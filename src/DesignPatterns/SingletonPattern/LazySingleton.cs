using System;

namespace SingletonPattern
{
    // Singleton generyczny z Lazy<T>
    public class LazySingleton<T>
        where T : new()
    {
        private static readonly Lazy<T> lazy = new Lazy<T>(() => new T());

        public static T Instance => lazy.Value;
    }
}
