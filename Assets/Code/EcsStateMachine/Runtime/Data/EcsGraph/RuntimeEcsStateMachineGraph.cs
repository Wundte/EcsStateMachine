using System.Collections.Generic;
using Generated;
using Sirenix.OdinInspector;

namespace Code.EcsStateMachine.Runtime.Data.EcsGraph
{
    #pragma warning disable UAC1009 // Suppress warning from unity serializer 
    
    [System.Serializable]
    public sealed class RuntimeEcsStateMachineGraph : SerializedScriptableObject
    {
        /// <summary>
        /// All runtime state nodes with hash as key.
        /// Using dictionary we circumvent serialization depth limit allowing for loops inside graph
        /// </summary>
        public Dictionary<int, RuntimeStateNode> AllRuntimeStateNodes = new ();

        /// <summary>
        /// Firs Ecs State to be active on game load. 
        /// </summary>
        public EcsStatesIds DefaulState;
    }
}