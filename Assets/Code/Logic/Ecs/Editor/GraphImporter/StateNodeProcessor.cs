using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Code.Data.Ecs.EcsStateMachine;
using Code.Data.Ecs.EcsStateMachine.Editor.GraphNodes;
using Generated;
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
                    defaultNextState = defaultNextStateNode.GetHashCode();
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
                    possibleNextStates.Add(possibleNextStateNode.GetHashCode());
                }
            }
            
            return new RuntimeStateNode
            {
                Name = name,
                DefaultNextState = defaultNextState,
                PossibleNextStates = possibleNextStates,
                OnStateEnterSystems = GetBlockNodeValues<OnStateEnterSystemsBlockNode, EcsRunSystemsIds>(stateNode),
                Features = GetBlockNodeValues<FeaturesBlockNode, EcsFeatureIds>(stateNode),
            };
        }
        
        private static List<TValue> GetBlockNodeValues<TBlock, TValue>(StateNode stateNode)
            where TBlock : BlockNode
        {
            var result = new List<TValue>();

            if (stateNode is not ContextNode contextNode)
            {
                return result;
            }

            foreach (var blockNode in contextNode.BlockNodes)
            {
                if (blockNode is TBlock block)
                {
                    foreach (var port in block.GetInputPorts())
                    {
                        if (port.TryGetValue(out TValue value))
                        {
                            result.Add(value);
                        }
                    }
                }
            }

            return result;
        }
    }
}