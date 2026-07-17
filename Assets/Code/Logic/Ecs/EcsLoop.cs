using System.Collections.Generic;
using Code.Data.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Logic.Ecs
{
    [DisallowMultipleComponent]
    public sealed class EcsLoop : MonoBehaviour
    {
        private Dictionary<SystemType, EcsSystems> _systems;
        
        public void Init(Dictionary<SystemType, EcsSystems> systems)
        {
            _systems = systems;
        }
        
        private void Update()
        {
            _systems[SystemType.Run].Run();
        }
    }
}