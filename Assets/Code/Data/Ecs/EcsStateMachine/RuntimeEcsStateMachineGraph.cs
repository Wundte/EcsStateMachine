using System.Collections.Generic;
using UnityEngine;

namespace Code.Data.Ecs.EcsStateMachine
{
    [System.Serializable]
    public sealed class RuntimeEcsStateMachineGraph : ScriptableObject
    {
        /// <summary>
        /// All runtime state nodes with hash as key. 
        /// </summary>
        public Dictionary<int, RuntimeStateNode> AllRuntimeStateNodes = new ();
    }
}