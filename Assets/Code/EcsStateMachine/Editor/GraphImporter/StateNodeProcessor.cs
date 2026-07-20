using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Code.EcsStateMachine.Editor.CodeGeneration.Utils;
using Code.EcsStateMachine.Editor.EcsStateMachineGraph.GraphNodes;
using Code.EcsStateMachine.Runtime.Data.EcsGraph;
using Code.EcsStateMachine.Runtime.Generated;
using Code.EcsStateMachine.Runtime.Logic.Abstractions;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Code.EcsStateMachine.Editor.GraphImporter
{
    public static class StateNodeProcessor
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RuntimeStateNode GetNew(StateNode stateNode)
        {
            // Get options
            stateNode.GetNodeOptionByName(StateNode.StateName).TryGetValue(out string name);
            stateNode.GetNodeOptionByName(StateNode.IsDefaultState).TryGetValue(out bool isDefaultState);

            // Process out port
            // Precess default next state out port
            var outDefaultNextStatePort = stateNode.GetOutputPortByName(StateNode.DefaultNextState);
            var portsConnectedToDefaultNextStatePort = new List<IPort>();
            outDefaultNextStatePort.GetConnectedPorts(portsConnectedToDefaultNextStatePort);

            var defaultNextState = 0;
            foreach (var connectedPort in portsConnectedToDefaultNextStatePort)
            {
                var connectedNode = connectedPort.GetNode();
                if (connectedNode is StateNode defaultNextStateNode)
                {
                    defaultNextStateNode.GetNodeOptionByName(StateNode.StateName).TryGetValue(out string nextStateName);
                    defaultNextState = StableId.Get(nextStateName);
                }
            }
            
            // Precess possible next states out port
            var outPossibleNextStatesPort = stateNode.GetOutputPortByName(StateNode.DefaultNextState);
            var portsConnectedToPossibleNextStatesPort = new List<IPort>();
            outPossibleNextStatesPort.GetConnectedPorts(portsConnectedToPossibleNextStatesPort);
            
            var possibleNextStates = new List<int>();
            foreach (var connectedPort in portsConnectedToPossibleNextStatesPort)
            {
                var connectedNode = connectedPort.GetNode();
                if (connectedNode is StateNode possibleNextStateNode)
                {
                    possibleNextStateNode.GetNodeOptionByName(StateNode.StateName).TryGetValue(out string nextStateName);
                    possibleNextStates.Add(StableId.Get(nextStateName));
                }
            }
            
            return new RuntimeStateNode
            {
                Id = StableId.Get(name),
                IsDefaultState = isDefaultState,
                DefaultNextState = defaultNextState,
                PossibleNextStates = possibleNextStates,
                OnStateEnterSystems = GetBlockData<OnStateEnterSystemsBlockNode, EcsStateChangeSystemsIds, IEcsStateChangeSystem>(
                    stateNode, 
                    EcsStateChangeSystemsFactory.Create,
                    name),
                Features = GetBlockData<FeaturesBlockNode, EcsFeatureIds, EcsFeature>(
                    stateNode, 
                    EcsFeatureFactory.Create,
                    name),
                OnStateExitSystems = GetBlockData<OnStateExitSystemBlockNode, EcsStateChangeSystemsIds, IEcsStateChangeSystem>(
                    stateNode, 
                    EcsStateChangeSystemsFactory.Create,
                    name)
            };
        }
        
        private static List<TResult> GetBlockData<TBlock, TEnum, TResult>(
            StateNode stateNode,
            Func<TEnum, TResult> factory,
            string nodeName)
            where TBlock : BlockNode
            where TEnum : struct, Enum
        {
            var result = new List<TResult>();
            var addedTypes = new HashSet<Type>();

            if (stateNode is not ContextNode contextNode)
            {
                return result;
            }

            var blockTypesCounter = new Dictionary<Type, int>();

            foreach (var blockNode in contextNode.BlockNodes)
            {
                if (blockNode is not TBlock block)
                {
                    continue;
                }

                var blockType = block.GetType();
                blockTypesCounter[blockType] = blockTypesCounter.GetValueOrDefault(blockType) + 1;

                foreach (var port in block.GetInputPorts())
                {
                    if (!port.TryGetValue(out TEnum value))
                    {
                        continue;
                    }

                    if (!Enum.IsDefined(typeof(TEnum), value))
                    {
                        Debug.LogWarning($"{nodeName} State contains empty field.");
                        continue;
                    }

                    var product = factory(value);

                    if (product == null)
                    {
                        continue;
                    }

                    var productType = product.GetType();

                    // We check for adding duplicates systems or features, they won't cause any problems, but are probably not what is intended
                    if (!addedTypes.Add(productType))
                    {
                        Debug.LogWarning($"{nodeName} State contains duplicate {productType.Name}. Duplicates won't be added to ECS schedule.");
                        continue;
                    }

                    result.Add(product);
                }
            }

            // Duplicate block nodes won't cause any problems, but they are probably not what is intended
            foreach (var block in blockTypesCounter)
            {
                if (block.Value > 1)
                {
                    Debug.LogWarning($"{nodeName} State contains duplicate BlockNode of type {block.Key.Name}.");
                }
            }

            return result;
        }
    }
}