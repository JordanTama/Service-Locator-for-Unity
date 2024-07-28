using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace JordanTama.ServiceLocator
{
    public static class Locator
    {
        public delegate void ServiceReadyDelegate(IService service);
        
        private static readonly Dictionary<string, IService> Services = new();
        private static readonly Dictionary<string, IConfiguration> Configurations = new();

        #region Registration
        
        public static T Register<T>(T service, ServiceReadyDelegate onRegistered = null) where T : IService
        {
            RegisterAsync(service).ContinueWith(() =>
            {
                onRegistered?.Invoke(service);
            });
            
            return service;
        }

        public static async UniTask RegisterAsync<T>(T service) where T : IService
        {
            if (typeof(T) == typeof(IService))
            {
                Debug.LogError("Tried to register service of type IService. Register service as its actual type, rather than an IService.");
                return;
            }

            var key = GetServiceKey<T>();
            if (Services.ContainsKey(key))
            {
                if (Application.isPlaying && !Application.isEditor)
                    Debug.LogWarning($"Service of type {typeof(T).Name} already registered!");
                
                return;
            }

            if (service is IConfigurableService configurable)
            {
                var configuration = await configurable.GetConfiguration();
                Configurations.Add(key, configuration);
            }

            Services.Add(key, service);
            service.OnRegistered();
        }

        public static void Unregister<T>(T service) where T : IService
        {
            var key = GetServiceKey<T>();

            Configurations.Remove(key);
            
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
        
        public static bool Get<T>(out T service) where T : IService
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

        public static bool TryGetConfiguration<T>(out IConfiguration configuration) where T : IConfigurableService
        {
            var key = GetServiceKey<T>();
            if (Configurations.TryGetValue(key, out configuration))
                return true;
            
            Debug.LogError($"No configuration found for service \"{key}\". It may still be loading...");
            return false;

        }
        
        #endregion
        
        #region Utility

        private static string GetServiceKey<T>() where T : IService => typeof(T).FullName;
        
        #endregion
    }
}
