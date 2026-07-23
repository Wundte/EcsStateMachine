using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Code.EcsStateMachine.Editor.CodeGeneration.SourceGenerationData.Utils
{
    /// <summary>
    /// Finds types available for code generation.
    /// </summary>
    public static class TypeFinder
    {
        /// <summary>
        /// Finds valid non-abstract types derived from specified type.
        /// </summary>
        public static List<Type> FindDerivedTypes<T>()
        {
            return TypeCache.GetTypesDerivedFrom<T>()
                .Where(t =>
                    t.IsClass
                    && !t.IsAbstract
                    && !t.IsGenericTypeDefinition
                    && !t.ContainsGenericParameters)
                .OrderBy(t => t.Name)
                .ToList();
        }
    }
}