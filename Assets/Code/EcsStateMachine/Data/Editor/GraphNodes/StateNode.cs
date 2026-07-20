using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Code.EcsStateMachine.Data.Editor.GraphNodes
{
    [System.Serializable]
    public sealed class StateNode : ContextNode
    {
        public const string StateName = "StateName";
        public const string IsDefaultState = "IsDefaultState";
        
        public const string In = "In";
        
        public const string DefaultNextState = "DefaultNextState";
        public const string PossibleNextStates = "PossibleNextStates";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<string>(StateName).Build();
            context.AddOption<bool>(IsDefaultState).Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(In).Build();
            
            context.AddOutputPort(DefaultNextState).Build();
            context.AddOutputPort(PossibleNextStates).Build();
        }
        
        public override void OnEnable()
        {
            DefaultColor = Color.blue;
        }
    }
}