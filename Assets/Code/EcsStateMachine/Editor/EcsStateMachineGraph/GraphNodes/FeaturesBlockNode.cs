using System;
using Code.EcsStateMachine.Editor.EcsStateMachineGraph.Constants;
using Code.EcsStateMachine.Runtime.Generated;
using Unity.GraphToolkit.Editor;

namespace Code.EcsStateMachine.Editor.EcsStateMachineGraph.GraphNodes
{
    [UseWithContext(typeof(StateNode))] [System.Serializable]
    public sealed class FeaturesBlockNode : BlockNode
    {
        private const string NumberOfFeatures = "NumberOfFeatures";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(NumberOfFeatures)
                .WithDisplayName("Number of Features")
                .WithDefaultValue(1)
                .Delayed() 
                .Build();
        }
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            var numberOfFeaturesOption = GetNodeOptionByName(NumberOfFeatures);
        
            if (numberOfFeaturesOption != null && numberOfFeaturesOption.TryGetValue<int>(out var number))
            {
                // Avoid big numbers just in case. 
                number = Math.Clamp(number, NodesConstants.MinNumberOfElements, NodesConstants.MaxNumberOfElements);

                for (var i = 0; i < number; i++)
                {
                    context.AddInputPort<EcsFeatureIds>($"{i}").Build();
                }
            }
        }
        
        public override void OnEnable()
        {
            Title = "State Features:";
        }
    }
}