using System;

namespace JordanTama.ServiceLocator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class ServiceConfigurationAttribute : Attribute
    {
        private Type _targetType;

        public ServiceConfigurationAttribute(Type targetType)
        {
            _targetType = targetType;
        }
    }
}
