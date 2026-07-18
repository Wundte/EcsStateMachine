using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Code.Data.Ecs.EcsStateMachine;
using Code.Data.Ecs.EcsStateMachine.Editor.GraphNodes;
using Code.Logic.CodeGeneration.Editor.Utils;
using Code.Logic.Ecs.Features;
using Generated;
using Leopotam.EcsLite;
using Unity.GraphToolkit.Editor;

namespace Code.Logic.Ecs.Editor.GraphImporter
{
    public static class StateNodeProcessor
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RuntimeStateNode GetNew(StateNode stateNode)
        {
            // Get options
            stateNode.GetNodeOptionByName(StateNode.StateName).TryGetValue(out string name);

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
                DefaultNextState = defaultNextState,
                PossibleNextStates = possibleNextStates,
                OnStateEnterSystems = GetBlockData<OnStateEnterSystemsBlockNode, EcsRunSystemsIds, IEcsRunSystem>(
                    stateNode, 
                    EcsRunSystemsFactory.Create),
                Features = GetBlockData<FeaturesBlockNode, EcsFeatureIds, EcsFeature>(
                    stateNode, 
                    EcsFeatureFactory.Create),
            };
        }
        
        private static List<TResult> GetBlockData<TBlock, TEnum, TResult>(StateNode stateNode, Func<TEnum, TResult> factory)
            where TBlock : BlockNode
            where TEnum : struct, Enum
        {
            var result = new List<TResult>();

            if (stateNode is not ContextNode contextNode)
            {
                return result;
            }

            foreach (var blockNode in contextNode.BlockNodes)
            {
                if (blockNode is not TBlock block)
                {
                    continue;
                }

                foreach (var port in block.GetInputPorts())
                {
                    if (port.TryGetValue(out TEnum value))
                    {
                        result.Add(factory(value));
                    }
                }
            }

            return result;
        }
    }
}