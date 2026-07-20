using System.Collections.Generic;
using Code.EcsStateMachine.CodeGeneration.Editor.Generation.Types;
using Code.EcsStateMachine.CodeGeneration.Editor.Output;
using Code.EcsStateMachine.CodeGeneration.Editor.SourceGenerationData.Enums;
using Code.EcsStateMachine.Data.EcsGraph;
using Code.EcsStateMachine.Data.Editor.EcsGraph;
using Code.EcsStateMachine.Data.Editor.GraphNodes;
using Generated;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Code.EcsStateMachine.Logic.Editor.GraphImporter
{
    [ScriptedImporter(1, "ecsgraph")]
    public sealed class EcsStateMachineGraphImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var editorEventGraph = GraphDatabase.LoadGraphForImporter<EcsStateMachineGraph>(ctx.assetPath);
            
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
                    runtimeEcsStateMachineGraph.DefaulState = (EcsStatesIds)node.Id;
                    break;
                }
            }
            
            // Generate enum from imported states names
            FileWriter.Write(
                "EcsStatesIds.cs", // TODO: Fix this, make a constant
                EnumGenerator.GenerateEnum(new EnumDefinition
                {
                    Namespace = "Generated", // TODO: Fix this, make a constant
                    Name = "EcsStatesIds", // TODO: Fix this, make a constant
                    Values = values
                }));
            
            ctx.AddObjectToAsset(ctx.assetPath, runtimeEcsStateMachineGraph);
            
            Debug.Log("<color=#00C853>Ecs Graph successfully imported</color>");
        }
    }
}