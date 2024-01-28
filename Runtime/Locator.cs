using System.Collections.Generic;
using UnityEngine;

namespace JordanTama.ServiceLocator
{
    public static class Locator
    {
        private static readonly Dictionary<string, IService> Services = new();

        public static T Register<T>(T service) where T : IService
        {
            if (typeof(T) == typeof(IService))
            {
                Debug.LogError("Tried to register service of type IService. Register service as its actual type, rather than an IService.");
                return default;
            }

            string key = GetServiceKey<T>();
            if (Services.ContainsKey(key))
            {
                Debug.LogError($"Service of type {typeof(T).Name} already registered!");
                return default;
            }

            Services.Add(key, service);
            service.OnRegistered();
            return service;
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

        public static bool Get<T>(out T service) where T : IService
        {
            string key = GetServiceKey<T>();
            if (!Services.ContainsKey(key))
            {
                service = default;
                return false;
            }

            service = (T) Services[key];
            return true;
        }

        public static T Get<T>() where T : IService
        {
            string key = GetServiceKey<T>();
            T service = Services.ContainsKey(key) ? (T)Services[key] : default;
            return service;
        }

        public static bool IsRegistered<T>() where T : IService => Services.ContainsKey(GetServiceKey<T>());

        private static string GetServiceKey<T>() where T : IService => typeof(T).FullName;
    }
}
