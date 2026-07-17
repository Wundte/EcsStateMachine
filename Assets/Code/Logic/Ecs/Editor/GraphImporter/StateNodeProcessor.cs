using System.Runtime.CompilerServices;
using Code.Data.Ecs.EcsStateMachine;
using Code.Data.Ecs.EcsStateMachine.Editor.GraphNodes;

namespace Code.Logic.Ecs.Editor.GraphImporter
{
    public static class StateNodeProcessor
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RuntimeStateNode GetNew(StateNode stateNode)
        {
            stateNode.GetNodeOptionByName(StateNode.StateName).TryGetValue(out string name);

            var runtimeStateNode = new RuntimeStateNode
            {
                Name = name
            };
            
            //
            // var runtimeRootNode = new RuntimeRootNode
            // {
            //     EventWeight = weight,
            //     PotentialFirstWindows = new List<int>()
            // };
            //
            // // Process out port
            // var outPort = stateNode.GetOutputPortByName(EventRootNode.Out);
            // var outConnectedPorts = new List<IPort>();
            // outPort.GetConnectedPorts(outConnectedPorts);
            // foreach (var connectedPort in outConnectedPorts)
            // {
            //     var connectedNode = connectedPort.GetNode();
            //     if (connectedNode is EventWindowNode eventWindowNode)
            //     {
            //         runtimeRootNode.PotentialFirstWindows.Add(eventWindowNode.GetHashCode());
            //     }
            // }
            //
            // return runtimeRootNode;

            return runtimeStateNode;
        }
    }
}