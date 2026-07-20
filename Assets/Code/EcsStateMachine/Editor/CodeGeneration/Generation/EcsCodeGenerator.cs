using System;
using System.Collections.Generic;
using System.Linq;
using Code.EcsStateMachine.Editor.CodeGeneration.Generation.Types;
using Code.EcsStateMachine.Editor.CodeGeneration.Output;
using Code.EcsStateMachine.Editor.CodeGeneration.SourceGenerationData.Enums;
using Code.EcsStateMachine.Editor.CodeGeneration.Utils;
using UnityEditor;

namespace Code.EcsStateMachine.Editor.CodeGeneration.Generation
{
    /// <summary>
    /// Generates ECS-related code files such as enums and factories.
    /// </summary>
    public sealed class EcsCodeGenerator
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
        /// Creates enum definition from provided types.
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
        /// Finds valid types for code generation.
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
        /// Creates mapping between generated names and full type names.
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
        /// Generates and writes enum source file.
        /// </summary>
        public void WriteEnum(EnumDefinition enumDef)
        {
            var enumCode = EnumGenerator.GenerateEnum(enumDef);
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