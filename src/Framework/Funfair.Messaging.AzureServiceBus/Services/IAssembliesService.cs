using System.Reflection;

namespace Funfair.Messaging.AzureServiceBus.Services;

public interface IAssembliesService
{
    IEnumerable<Type> ReturnTypes();
    IEnumerable<Assembly> ReturnAssemblies();
}