using System.Collections.Generic;
using Code.Logic.Ecs.Features;
using Code.Logic.Ecs.Interfaces;

namespace Code.Data.Ecs.EcsStateMachine
{
    [System.Serializable]
    public sealed class RuntimeStateNode
    {
        public int Id;
        
        // Use hash code instead of references to circumvent serialization depth limitations.
        public int DefaultNextState;
        public List<int> PossibleNextStates;
        
        public List<IEcsStateChangeSystem> OnStateEnterSystems;
        public List<EcsFeature> Features;
        public List<IEcsStateChangeSystem> OnStateExitSystems;
    }
}