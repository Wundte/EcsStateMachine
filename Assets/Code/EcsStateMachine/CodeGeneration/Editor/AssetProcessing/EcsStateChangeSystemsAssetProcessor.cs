using Code.EcsStateMachine.CodeGeneration.Editor.Generation;
using Code.EcsStateMachine.CodeGeneration.Editor.Generation.Factories;
using Code.EcsStateMachine.CodeGeneration.Editor.Output;
using Code.EcsStateMachine.Logic.Abstractions;
using UnityEditor;
using UnityEngine;

namespace Code.EcsStateMachine.CodeGeneration.Editor.AssetProcessing
{
    public class EcsStateChangeSystemsAssetProcessor : AssetPostprocessor
    {
        private static readonly EcsCodeGenerator Generator = new EcsCodeGenerator(
            enumName: "EcsStateChangeSystemsIds",
            enumNamespace: "Generated",
            outputFileName: "EcsStateChangeSystemsIds.cs");
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Generate();
        }

        [MenuItem("Tools/Generate ECS Systems")]
        public static void Generate()
        {
            var systemTypes = EcsCodeGenerator.FindTypesDerivedFrom<IEcsStateChangeSystem>();
            
            var enumDefinition = Generator.CreateEnumDefinition(systemTypes);
            Generator.WriteEnum(enumDefinition);
            
            var systemTypeMap = EcsCodeGenerator.CreateTypeMap(systemTypes);
            var factoryCode = EcsStateChangeSystemsFactoryGenerator.GenerateFactory(systemTypeMap);
            
            FileWriter.Write("EcsRunSystemsFactory.cs", factoryCode);

            AssetDatabase.Refresh();
            
            Debug.Log("<color=#00C853>EcsOnStateChangeSystemsIds and Factory successfully generated</color>");
        }
    }
}