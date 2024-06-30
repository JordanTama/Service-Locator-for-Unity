using System.Collections.Generic;
using UnityEngine;

namespace JordanTama.ServiceLocator
{
    public static class Locator
    {
        private static readonly Dictionary<string, IService> Services = new();
        private static readonly Dictionary<string, IConfiguration> Configurations = new();

        #region Registration
        
        public static T Register<T>(T service) where T : IService
        {
            if (typeof(T) == typeof(IService))
            {
                Debug.LogError("Tried to register service of type IService. Register service as its actual type, rather than an IService.");
                return default;
            }

            var key = GetServiceKey<T>();
            if (Services.ContainsKey(key))
            {
                if (Application.isPlaying && !Application.isEditor)
                    Debug.LogWarning($"Service of type {typeof(T).Name} already registered!");
                
                return (T)Services[key];
            }

            Services.Add(key, service);

            if (service is IConfigurableService configurable)
            {
                
                Configurations.Add(key, configurable.GetConfiguration());
            }
            
            service.OnRegistered();
            return service;
        }

        public static void Unregister<T>(T service) where T : IService
        {
            var key = GetServiceKey<T>();
            if (!Services.ContainsKey(key))
            {
                Debug.LogError($"Service of type {typeof(T).Name} not registered!");
                return;
            }

            Services.Remove(key);
            service.OnUnregistered();
        }

        public static bool IsRegistered<T>() where T : IService => Services.ContainsKey(GetServiceKey<T>());
        
        #endregion
        
        #region Retrieval

        public static T Get<T>() where T : IService
        {
            var key = GetServiceKey<T>();
            var service = Services.ContainsKey(key) ? (T)Services[key] : default;
            return service;
        }
        
        public static bool TryGet<T>(out T service) where T : IService
        {
            var key = GetServiceKey<T>();
            if (!Services.ContainsKey(key))
            {
                service = default;
                return false;
            }

            service = (T) Services[key];
            return true;
        }

        #endregion
        
        #region Configurable Services

        public static IConfiguration GetConfiguration<T>() where T : IConfigurableService
        {
            
        }
        
        #endregion
        
        #region Utility

        private static string GetServiceKey<T>() where T : IService => typeof(T).FullName;
        
        #endregion
    }
}
