using Code.EcsStateMachine.Editor.CodeGeneration.Generation;
using Code.EcsStateMachine.Editor.CodeGeneration.Generation.Factories;
using Code.EcsStateMachine.Editor.CodeGeneration.Output;
using Code.EcsStateMachine.Runtime.Logic.Abstractions;
using UnityEditor;
using UnityEngine;

namespace Code.EcsStateMachine.Editor.CodeGeneration.AssetProcessing
{
    public class EcsStateChangeSystemsAssetProcessor : AssetPostprocessor
    {
        private static readonly EcsCodeGenerator Generator = new EcsCodeGenerator(
            enumName: "EcsStateChangeSystemsIds",
            enumNamespace: "Code.EcsStateMachine.Runtime.Generated",
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