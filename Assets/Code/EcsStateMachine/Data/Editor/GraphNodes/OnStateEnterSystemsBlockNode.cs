using System;
using Code.EcsStateMachine.Data.Editor.Constants;
using Generated;
using Unity.GraphToolkit.Editor;

namespace Code.EcsStateMachine.Data.Editor.GraphNodes
{
    [UseWithContext(typeof(StateNode))] [System.Serializable]
    public class OnStateEnterSystemsBlockNode : BlockNode
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
            Title = "On State Enter Systems:";
        }
    }
}