using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Code.Data.Ecs.EcsStateMachine
{
    [System.Serializable]
    public sealed class RuntimeEcsStateMachineGraph : SerializedScriptableObject
    {
        /// <summary>
        /// All runtime state nodes with hash as key. 
        /// </summary>
        public Dictionary<int, RuntimeStateNode> AllRuntimeStateNodes = new ();
    }
}