using System.Collections.Generic;
using Code.Data.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Logic.Ecs
{
    [DisallowMultipleComponent]
    public sealed class EcsLoop : MonoBehaviour
    {
        private EcsSystems _updateSystems;
        
        public void Init(Dictionary<int, EcsSystems> systems)
        {
            _updateSystems = systems[(int)SystemType.Run];
        }
        
        private void Update()
        {
            _updateSystems.Run();
        }
    }
}