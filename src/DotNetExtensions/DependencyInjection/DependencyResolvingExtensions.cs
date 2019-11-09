using DotNetExtensions.Common;
using DotNetExtensions.Validation;
using System;
using System.Linq;
using System.Reflection;

namespace DotNetExtensions.DependencyInjection
{
    public static class DependencyResolvingExtensions
    {
        public static TResolver AddConvetionallyNamedServices<TResolver>(
            this TResolver resolver,
            Action<TResolver, Type, Type> addDependencyAction,
            Assembly assembly = default)
            where TResolver : class
        {
            resolver.ThrowIfNull("The passed dependency resolver cannot be null.");

            addDependencyAction.ThrowIfNull(
                "The passed action for adding dependency cannot be null.");

            if (assembly.IsNull())
            {
                assembly = Assembly.GetCallingAssembly();
            }

            assembly
                .GetTypes()
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && !t.IsGenericType
                    && t.GetInterfaces()
                        .Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Interface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .ForEach(s => addDependencyAction(resolver, s.Interface, s.Implementation));

            return resolver;
        }
    }
}
