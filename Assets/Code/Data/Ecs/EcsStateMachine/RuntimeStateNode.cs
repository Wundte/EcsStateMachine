using System.Collections.Generic;
using Generated;

namespace Code.Data.Ecs.EcsStateMachine
{
    [System.Serializable]
    public sealed class RuntimeStateNode
    {
        public string Name;
        
        // Use hash code instead of references to circumvent serialization depth limitations.
        public int DefaultNextState;
        public List<int> PossibleNextStates;
        
        public List<EcsRunSystemsIds> OnStateEnterSystems;
        public List<EcsFeatureIds> Features;
    }
}