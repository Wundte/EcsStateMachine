using System.Collections.Generic;
using Code.EcsStateMachine.Logic.Abstractions;

namespace Code.EcsStateMachine.Data.EcsGraph
{
    [System.Serializable]
    public sealed class RuntimeStateNode
    {
        public int Id;
        public bool IsDefaultState;
        
        // Use hash code instead of references to circumvent serialization depth limitations.
        public int DefaultNextState;
        public List<int> PossibleNextStates;
        
        public List<IEcsStateChangeSystem> OnStateEnterSystems;
        public List<EcsFeature> Features;
        public List<IEcsStateChangeSystem> OnStateExitSystems;
    }
}