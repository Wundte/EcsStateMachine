using System.Collections.Generic;
using Code.Data.Ecs.EcsStateMachine;
using Code.Data.Ecs.EcsStateMachine.Editor.EcsGraph;
using Code.Data.Ecs.EcsStateMachine.Editor.GraphNodes;
using Code.Logic.CodeGeneration.Editor.Generation.Types;
using Code.Logic.CodeGeneration.Editor.Output;
using Code.Logic.CodeGeneration.Editor.SourceGenerationData.Enums;
using Code.Logic.CodeGeneration.Editor.Utils;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Code.Logic.Ecs.Editor.GraphImporter
{
    [ScriptedImporter(1, "ecsgraph")]
    public sealed class EcsStateMachineGraphImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var editorEventGraph = GraphDatabase.LoadGraphForImporter<EcsStateMachineGraph>(ctx.assetPath);
            
            var runtimeEcsStateMachineGraph = ScriptableObject.CreateInstance<RuntimeEcsStateMachineGraph>();
            
            // Select all nodes
            // Using dictionary we circumvent serialization depth limit allowing for loops inside graph
            var allNodes = new Dictionary<int, INode>();
            foreach (var node in editorEventGraph.GetNodes())
            {
                allNodes.Add(node.GetHashCode(), node);
            }
            
            var values = new List<EnumValue>();
            foreach (var (id, node) in allNodes)
            {
                if (node is StateNode stateNode)
                {
                    var newRuntimeStateNode = StateNodeProcessor.GetNew(stateNode);
                    runtimeEcsStateMachineGraph.AllRuntimeStateNodes.Add(id, newRuntimeStateNode);
                    
                    values.Add(new EnumValue
                    {
                        Name = newRuntimeStateNode.Name,
                        Value = StableId.Get(newRuntimeStateNode.Name)
                    });
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