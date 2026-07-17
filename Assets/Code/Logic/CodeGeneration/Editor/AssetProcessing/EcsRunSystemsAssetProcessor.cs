using Code.Logic.CodeGeneration.Editor.Generation;
using Code.Logic.CodeGeneration.Editor.Generation.Factories;
using Code.Logic.CodeGeneration.Editor.Output;
using Leopotam.EcsLite;
using UnityEditor;

namespace Code.Logic.CodeGeneration.Editor.AssetProcessing
{
    public class EcsRunSystemsAssetProcessor : AssetPostprocessor
    {
        private static readonly EcsCodeGenerator Generator = new EcsCodeGenerator(
            enumName: "EcsRunSystemsIds",
            enumNamespace: "Generated",
            outputFileName: "EcsRunSystemsIds.cs");

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Generate();
        }

        [MenuItem("Tools/Generate ECS Systems")]
        public static void Generate()
        {
            var systemTypes = EcsCodeGenerator.FindTypesDerivedFrom<IEcsRunSystem>();
            
            var enumDefinition = Generator.CreateEnumDefinition(systemTypes);
            Generator.WriteEnum(enumDefinition);
            
            var systemTypeMap = EcsCodeGenerator.CreateTypeMap(systemTypes);
            var factoryCode = EcsRunSystemFactoryGenerator.GenerateFactory(systemTypeMap);
            
            FileWriter.Write("EcsRunSystemsFactory.cs", factoryCode);

            AssetDatabase.Refresh();
        }
    }
}