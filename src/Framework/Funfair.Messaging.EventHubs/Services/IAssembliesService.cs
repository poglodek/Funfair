using System.Reflection;

namespace Funfair.Messaging.EventHubs.Services;

internal interface IAssembliesService
{
    IEnumerable<Type> ReturnTypes();
    IEnumerable<Assembly> ReturnAssemblies();
}