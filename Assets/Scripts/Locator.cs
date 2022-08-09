using System;
using System.Collections.Generic;

namespace Internal
{
    public static class Locator
    {
        private static Dictionary<Type, object> map = new Dictionary<Type, object>();

        public static void Register<T>(object   @object) => Register(typeof(T), @object);
        public static void Unregister<T>(object @object) => Unregister(typeof(T), @object);

        public static void Register(Type type, object @object)
        {
            map[type] = @object;
            onRegister?.Invoke(type, @object);
        }

        public static void Unregister(Type type, object @object)
        {
            if (map.ContainsKey(type) && map[type] == @object)
            {
                map[type] = null;
                onUnregister?.Invoke(type, @object);
            }
        }

        public static T GetObject<T>() where T : class
        {
            if (!map.ContainsKey(typeof(T))) return null;
            return (T) map[typeof(T)];
        }

        public static event Action<Type, object> onRegister;
        public static event Action<Type, object> onUnregister;
    }
}