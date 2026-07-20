using System;
using Code.EcsStateMachine.Editor.EcsStateMachineGraph.Constants;
using Code.EcsStateMachine.Runtime.Generated;
using Unity.GraphToolkit.Editor;

namespace Code.EcsStateMachine.Editor.EcsStateMachineGraph.GraphNodes
{
    [UseWithContext(typeof(StateNode))] [System.Serializable]
    public sealed class OnStateExitSystemBlockNode : BlockNode
    {
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
                // Avoid big numbers just in case. 
                number = Math.Clamp(number, NodesConstants.MinNumberOfElements, NodesConstants.MaxNumberOfElements);

                for (var i = 0; i < number; i++)
                {
                    context.AddInputPort<EcsStateChangeSystemsIds>($"{i}").Build();
                }
            }
        }
        
        public override void OnEnable()
        {
            Title = "On State Exit Systems:";
        }
    }
}