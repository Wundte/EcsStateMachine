using System;
using System.Collections.Generic;
using System.Linq;
using Code.Logic.CodeGeneration.Editor.Output;
using Code.Logic.CodeGeneration.Editor.SourceGenerationData.Enums;
using Code.Logic.CodeGeneration.Editor.Utils;
using UnityEditor;

namespace Code.Logic.CodeGeneration.Editor.Generation
{
    /// <summary>
    /// Generic code generator for ECS-related types (Systems, Features, etc.)
    /// Handles enum generation and file output in a reusable way.
    /// </summary>
    public class EcsCodeGenerator
    {
        private readonly string enumName;
        private readonly string enumNamespace;
        private readonly string outputFileName;

        public EcsCodeGenerator(
            string enumName,
            string enumNamespace,
            string outputFileName)
        {
            this.enumName = enumName;
            this.enumNamespace = enumNamespace;
            this.outputFileName = outputFileName;
        }

        /// <summary>
        /// Generate enum from list of types
        /// </summary>
        public EnumDefinition CreateEnumDefinition(List<Type> types)
        {
            var values = new List<EnumValue>();
            var generatedIds = new Dictionary<int, string>();
            var nameCounts = GetNameCounts(types);

            foreach (var type in types)
            {
                var fullName = type.FullName ?? type.Name;
                var id = StableId.Get(fullName);

                if (generatedIds.TryGetValue(id, out var existingType))
                {
                    throw new Exception($"ID collision between {existingType} and {fullName}");
                }

                generatedIds.Add(id, fullName);

                values.Add(new EnumValue
                {
                    Name = GetGeneratedName(type, nameCounts),
                    Value = id
                });
            }

            return new EnumDefinition
            {
                Namespace = enumNamespace,
                Name = enumName,
                Values = values
            };
        }

        /// <summary>
        /// Find all types derived from base type, filtered for code generation
        /// </summary>
        public static List<Type> FindTypesDerivedFrom<T>()
        {
            return TypeCache
                .GetTypesDerivedFrom<T>()
                .Where(t =>
                    t.IsClass
                    && !t.IsAbstract
                    && !t.IsGenericTypeDefinition
                    && !t.ContainsGenericParameters)
                .OrderBy(t => t.Name)
                .ToList();
        }

        /// <summary>
        /// Create type map (name -> full name) for factory generation
        /// </summary>
        public static Dictionary<string, string> CreateTypeMap(List<Type> types)
        {
            var result = new Dictionary<string, string>();
            var nameCounts = GetNameCounts(types);

            foreach (var type in types)
            {
                var name = GetGeneratedName(type, nameCounts);
                var fullName = type.FullName ?? type.Name;

                result.Add(name, fullName);
            }

            return result;
        }

        /// <summary>
        /// Write enum file to output
        /// </summary>
        public void WriteEnum(EnumDefinition enumDef)
        {
            var enumCode = Types.EnumGenerator.GenerateEnum(enumDef);
            FileWriter.Write(outputFileName, enumCode);
        }

        private static Dictionary<string, int> GetNameCounts(List<Type> types)
        {
            return types
                .GroupBy(t => t.Name)
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                );
        }

        private static string GetGeneratedName(Type type, Dictionary<string, int> nameCounts)
        {
            if (nameCounts[type.Name] == 1)
            {
                return type.Name;
            }

            var namespaceSuffix = type.Namespace?.Split('.').LastOrDefault() ?? "Global";
            
            return $"{namespaceSuffix}{type.Name}";
        }
    }
}