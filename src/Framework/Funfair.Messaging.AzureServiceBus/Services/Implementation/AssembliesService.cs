using System.Reflection;

namespace Funfair.Messaging.AzureServiceBus.Services.Implementation;

internal class AssembliesService : IAssembliesService
{
    public IEnumerable<Type> ReturnTypes()
    {
        return ReturnAssemblies().SelectMany(x => x.GetTypes());
    }

    public IEnumerable<Assembly> ReturnAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        var locations = assemblies.Where(x => !x.IsDynamic).Select(x => x.Location).ToArray();
        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
            .ToList();
        files.ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));

        return assemblies;
    }
}