using System;
using UnityEngine;

namespace JordanTama.ServiceLocator
{
    public interface IService
    {
        string LogPrefix { get; }
        
        void OnRegistered();
        void OnUnregistered();
        
        void WriteLine(string line, LogType logType = LogType.Log)
        {
            line = line.Insert(0, $"[{LogPrefix}] ");
            switch (logType)
            {
                case LogType.Error:
                    Debug.LogError(line);
                    break;
                
                case LogType.Assert:
                    Debug.LogAssertion(line);
                    break;
                
                case LogType.Warning:
                    Debug.LogWarning(line);
                    break;
                
                case LogType.Log:
                    Debug.Log(line);
                    break;
                
                case LogType.Exception:
                    Debug.LogException(new Exception(line));
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }
    }
}
