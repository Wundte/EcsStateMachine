using System.Collections.Generic;
using Code.EcsStateMachine.Runtime.Logic.Abstractions;

namespace Code.EcsStateMachine.Runtime.Data.EcsGraph
{
    /// <summary>
    /// Runtime representation of a single ECS state.
    /// </summary>
    [System.Serializable]
    public sealed class RuntimeStateNode
    {
        /// <summary>
        /// Stable hash identifier generated from state name.
        /// </summary>
        public int Id;

        public bool IsDefaultState;

        /// <summary>
        /// Stable hash identifier of the default next state.
        /// Generated from state name using StableId.
        /// Used instead of object reference to avoid Unity serialization depth limitations.
        /// </summary>
        public int DefaultNextState;

        /// <summary>
        /// Stable hash identifiers of possible next states.
        /// Generated from state names using StableId.
        /// </summary>
        public List<int> PossibleNextStates;

        public List<IEcsStateChangeSystem> OnStateEnterSystems;
        public List<EcsFeature> Features;
        public List<IEcsStateChangeSystem> OnStateExitSystems;
    }
}