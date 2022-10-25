using System.Collections.Generic;
using UnityEngine;

namespace JordanTama.ServiceLocator
{
    public static class Locator
    {
        private static readonly Dictionary<string, IService> Services = new();

        public static void Register<T>(T service) where T : IService
        {
            if (typeof(T) == typeof(IService))
            {
                Debug.LogError("Tried to register service of type IService. Register service as its actual type, rather than an IService.");
                return;
            }

            string key = GetServiceKey<T>();
            if (Services.ContainsKey(key))
            {
                Debug.LogError($"Service of type {typeof(T).Name} already registered!");
                return;
            }

            Services.Add(key, service);
            service.OnRegistered();
        }

        public static void Unregister<T>(T service) where T : IService
        {
            string key = GetServiceKey<T>();
            if (!Services.ContainsKey(key))
            {
                Debug.LogError($"Service of type {typeof(T).Name} not registered!");
                return;
            }

            Services.Remove(key);
            service.OnUnregistered();
        }

        public static void Get<T>(out T service) where T : IService
        {
            string key = GetServiceKey<T>();
            if (!Services.ContainsKey(key))
            {
                service = default;
                return;
            }

            service = (T) Services[key];
        }

        public static bool IsRegistered<T>() where T : IService => Services.ContainsKey(GetServiceKey<T>());

        private static string GetServiceKey<T>() where T : IService => typeof(T).FullName;
    }
}
