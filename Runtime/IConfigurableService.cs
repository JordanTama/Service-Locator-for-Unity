using Cysharp.Threading.Tasks;

namespace JordanTama.ServiceLocator
{
    public interface IConfigurableService : IService
    {
        UniTask<IConfiguration> GetConfiguration();
    }
}
