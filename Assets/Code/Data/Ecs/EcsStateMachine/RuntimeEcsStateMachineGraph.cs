using System.Collections.Generic;
using Generated;
using Sirenix.OdinInspector;

namespace Code.Data.Ecs.EcsStateMachine
{
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