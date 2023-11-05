using System.Reflection;

namespace Funfair.Messaging.AzureServiceBus.Services;

internal interface IAssembliesService
{
    IEnumerable<Type> ReturnTypes();
    IEnumerable<Assembly> ReturnAssemblies();
}