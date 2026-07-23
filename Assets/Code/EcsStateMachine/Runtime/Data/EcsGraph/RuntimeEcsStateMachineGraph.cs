using System.Collections.Generic;
using Generated;
using Sirenix.OdinInspector;

namespace Code.EcsStateMachine.Runtime.Data.EcsGraph
{
#pragma warning disable UAC1009 // Suppress warning from unity serializer about dictionaries serialization
    
    /// <summary>
    /// Runtime representation of ECS state machine graph.
    /// </summary>
    [System.Serializable]
    public sealed class RuntimeEcsStateMachineGraph : SerializedScriptableObject
    {
        /// <summary>
        /// Runtime state nodes indexed by their unique identifiers.
        /// Dictionary allows storing graph connections without serialization depth limitations.
        /// </summary>
        public Dictionary<int, RuntimeStateNode> AllRuntimeStateNodes = new ();

        /// <summary>
        /// Initial state activated when the game starts.
        /// </summary>
        public EcsStatesIds DefaultState;
    }
}