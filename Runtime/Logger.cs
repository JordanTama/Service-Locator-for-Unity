using UnityEngine;

namespace JordanTama.ServiceLocator
{
    public static class Logger
    {
        public static void Write<T>(string text) where T : IService
        {
            Debug.Log($"[{typeof(T)}] {text}");
        }

        public static void Write<T>(T service, string text) where T : IService
        {
            Debug.Log($"[{service.GetType()}] {text}");
        }
    }
}