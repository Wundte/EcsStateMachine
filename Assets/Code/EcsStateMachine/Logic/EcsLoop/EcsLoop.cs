using System.Collections.Generic;
using Code.EcsStateMachine.Data.Constants;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.EcsStateMachine.Logic.EcsLoop
{
    [DisallowMultipleComponent]
    public sealed class EcsLoop : MonoBehaviour
    {
        private EcsSystems _updateSystems;
        
        public void Init(Dictionary<int, EcsSystems> systems)
        {
            _updateSystems = systems[(int)EcsSystemType.Run];
        }
        
        private void Update()
        {
            _updateSystems.Run();
        }
    }
}