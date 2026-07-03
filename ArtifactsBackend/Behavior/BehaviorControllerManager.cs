using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArtifactsBackend.Behavior.Base;
using ArtifactsBackend.Core;

namespace ArtifactsBackend.Behavior;

public static class BehaviorControllerManager
{
    static BehaviorControllerManager()
    {
        IEnumerable<Type> typesToRegister = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetCustomAttribute<RegisterAttribute>()?.RegistryType == IBehaviorControllerConfiguration.REGISTRY);

        foreach (Type type in typesToRegister)
        {
            IBehaviorControllerConfiguration? config = Activator.CreateInstance(type) as IBehaviorControllerConfiguration;
            if (config is null)
            {
                throw new TypeLoadException($"Type {type.Name} has a Register assembly but no parameterless constructor.");
            }

            s_BehaviorControllerConfigurations.Add(config);
        }
    }

    public static IReadOnlyCollection<IBehaviorControllerConfiguration?> BehaviorControllerConfigurations => s_BehaviorControllerConfigurations;
    private static readonly List<IBehaviorControllerConfiguration?> s_BehaviorControllerConfigurations = [];
}