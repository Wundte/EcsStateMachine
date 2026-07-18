using Code.Logic.CodeGeneration.Editor.Generation;
using Code.Logic.CodeGeneration.Editor.Generation.Factories;
using Code.Logic.CodeGeneration.Editor.Output;
using Code.Logic.Ecs.Features;
using UnityEditor;
using UnityEngine;

namespace Code.Logic.CodeGeneration.Editor.AssetProcessing
{
    public class EcsFeatureAssetProcessor : AssetPostprocessor
    {
        private static readonly EcsCodeGenerator Generator = new EcsCodeGenerator(
            enumName: "EcsFeatureIds",
            enumNamespace: "Generated",
            outputFileName: "EcsFeatureIds.cs");

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Generate();
        }

        [MenuItem("Tools/Generate ECS Features")]
        public static void Generate()
        {
            var featureTypes = EcsCodeGenerator.FindTypesDerivedFrom<EcsFeature>();

            var enumDefinition = Generator.CreateEnumDefinition(featureTypes);
            Generator.WriteEnum(enumDefinition);

            var featureTypeMap = EcsCodeGenerator.CreateTypeMap(featureTypes);
            var factoryCode = EcsFeatureFactoryGenerator.GenerateFactory(featureTypeMap);

            FileWriter.Write("EcsFeatureFactory.cs", factoryCode);

            AssetDatabase.Refresh();
            
            Debug.Log("<color=#00C853>EcsFeatureIds and Factory successfully generated</color>");
        }
    }
}

