using System;
using Code.EcsStateMachine.Editor.EcsStateMachineGraph.Constants;
using Code.EcsStateMachine.Runtime.Generated;
using Unity.GraphToolkit.Editor;

namespace Code.EcsStateMachine.Editor.EcsStateMachineGraph.GraphNodes
{
    /// <summary>
    /// Graph node block for configuring systems executed on state enter.
    /// </summary>
    [UseWithContext(typeof(StateNode))] [System.Serializable]
    public class OnStateEnterSystemsBlockNode : BlockNode
    {
        /// <summary>
        /// Node option storing the number of system ports.
        /// </summary>
        private const string NumberOfSystem = "NumberOfSystem";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(NumberOfSystem)
                .WithDisplayName("Number of System")
                .WithDefaultValue(1)
                .Delayed() 
                .Build();
        }
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            var numberOfSystemsOption = GetNodeOptionByName(NumberOfSystem);
        
            if (numberOfSystemsOption != null && numberOfSystemsOption.TryGetValue<int>(out var number))
            {
                // Limit port count to keep node readable.
                number = Math.Clamp(number, NodesConstants.MinNumberOfPorts, NodesConstants.MaxNumberOfPorts);

                for (var i = 0; i < number; i++)
                {
                    context.AddInputPort<EcsStateChangeSystemsIds>($"{i}").Build();
                }
            }
        }
        
        public override void OnEnable()
        {
            Title = "On State Enter Systems:";
        }
    }
}