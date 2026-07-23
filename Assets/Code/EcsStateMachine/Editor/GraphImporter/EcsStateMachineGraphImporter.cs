using System.Collections.Generic;
using Code.EcsStateMachine.Editor.CodeGeneration.Generation.Types;
using Code.EcsStateMachine.Editor.CodeGeneration.Output;
using Code.EcsStateMachine.Editor.CodeGeneration.SourceGenerationData.Enums;
using Code.EcsStateMachine.Editor.EcsStateMachineGraph.GraphNodes;
using Code.EcsStateMachine.Editor.Settings;
using Code.EcsStateMachine.Runtime.Data.EcsGraph;
using Generated;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Code.EcsStateMachine.Editor.GraphImporter
{
    /// <summary>
    /// Imports ECS state machine graph assets into runtime graph data.
    /// </summary>
    [ScriptedImporter(1, "ecsgraph")]
    public sealed class EcsStateMachineGraphImporter : ScriptedImporter
    {
        /// <summary>
        /// Converts editor graph nodes into runtime state machine data
        /// and generates state identifiers.
        /// </summary>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var editorEventGraph = GraphDatabase.LoadGraphForImporter<EcsStateMachineGraph.EcsStateMachineGraph>(ctx.assetPath);
            
            var runtimeEcsStateMachineGraph = ScriptableObject.CreateInstance<RuntimeEcsStateMachineGraph>();
            
            var values = new List<EnumValue>();
            foreach (var node in editorEventGraph.GetNodes())
            {
                if (node is StateNode stateNode)
                {
                    var newRuntimeStateNode = StateNodeProcessor.GetNew(stateNode);
                    runtimeEcsStateMachineGraph.AllRuntimeStateNodes.Add(newRuntimeStateNode.Id, newRuntimeStateNode);
                    
                    stateNode.GetNodeOptionByName(StateNode.StateName).TryGetValue(out string stateName);
                    
                    values.Add(new EnumValue
                    {
                        Name = stateName,
                        Value = newRuntimeStateNode.Id
                    });
                }
            }

            foreach (var (_, node) in runtimeEcsStateMachineGraph.AllRuntimeStateNodes)
            {
                if (node.IsDefaultState)
                {
                    runtimeEcsStateMachineGraph.DefaultState = (EcsStatesIds)node.Id;
                    break;
                }
            }
            
            // Generate enum with state ids
            const string enumName = "EcsStatesIds";
            var enumDefinition = new EnumDefinition
            {
                Values = values,
                Name = enumName,
                Namespace = EcsStateMachineSettings.instance.GeneratedNamespace
            };
            FileWriter.Write($"{enumName}.cs", EnumSourceGenerator.Generate(enumDefinition));
            
            ctx.AddObjectToAsset(ctx.assetPath, runtimeEcsStateMachineGraph);
            
            EditorApplication.delayCall += AssetDatabase.Refresh;
            
            Debug.Log("<color=#00C853>Ecs Graph successfully imported</color>");
        }
    }
}