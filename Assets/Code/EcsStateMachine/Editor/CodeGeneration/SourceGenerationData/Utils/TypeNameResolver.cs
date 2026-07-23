using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.EcsStateMachine.Editor.CodeGeneration.SourceGenerationData.Utils
{
    /// <summary>
    /// Resolves type names for generated code and handles name collisions.
    /// </summary>
    internal static class TypeNameResolver
    {
        /// <summary>
        /// Creates a map containing the amount of occurrences for each type name.
        /// </summary>
        public static Dictionary<string, int> GetTypeNameOccurrences(IReadOnlyList<Type> types)
        {
            return types
                .GroupBy(t => t.Name)
                .ToDictionary(
                    group => group.Key,
                    group => group.Count());
        }

        /// <summary>
        /// Resolves a unique name for a type.
        /// Adds namespace suffix when multiple types have the same name.
        /// </summary>
        public static string Resolve(Type type, IReadOnlyDictionary<string, int> nameOccurrences)
        {
            if (nameOccurrences[type.Name] == 1)
            {
                return type.Name;
            }

            var namespaceSuffix = type.Namespace?.Split('.').LastOrDefault() ?? "Global";

            return $"{namespaceSuffix}{type.Name}";
        }
    }
}