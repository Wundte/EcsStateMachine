using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Code.Data.Ecs;
using Code.Data.Ecs.EcsStateMachine;
using Code.Logic.Ecs;
using Code.Logic.Services;
using Generated;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Code.Logic.GameLoad
{
    public sealed class EcsInitializer : IDisposable
    {
        private readonly Dictionary<SystemType, EcsSystems> _systems = new();
        
        private EcsWorld _ecsWorld;

        public EcsInitializer(
            List<object> injectParameters, 
            RuntimeEcsStateMachineGraph ecsStateMachineGraph, 
            EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;

            var systemTypes = Enum.GetValues(typeof(SystemType));
            foreach (var item in systemTypes)
            {
                _systems.Add((SystemType)item, new EcsSystems(_ecsWorld));
            }
            
            SelectSystems(ecsStateMachineGraph);
            
            InjectAndInitSystems(injectParameters);
        }
        
        public void StartEcsLoop()
        {
            var ecsLoopGameObject = new GameObject
            {
#if UNITY_EDITOR
                name = "EcsLoop"
#endif
            };
            var ecsLoop = ecsLoopGameObject.AddComponent<EcsLoop>();

            ecsLoop.Init(_systems);
            
            GameStateService.SetInitialState();
        }
        
        private void SelectSystems(RuntimeEcsStateMachineGraph ecsConfig)
        {
            var runSystems = _systems[SystemType.Run];
            foreach (var (_, stateNode) in ecsConfig.AllRuntimeStateNodes)
            {
                // Select run systems
                for (var j = 0; j < stateNode.OnStateEnterSystems.Count; j++)
                {
                    runSystems.Add(stateNode.OnStateEnterSystems[j]);
                }
                
                // Select system groups from features
                for (var j = 0; j < stateNode.Features.Count; j++)
                {
                    // This check is needed just in case the generation or id hash went wrong. 
                    if (!Enum.IsDefined(typeof(EcsStatesIds), stateNode.Id))
                    {
                        throw new Exception($"Unknown state id: {stateNode.Id}");
                    }
                    
                    var feature = stateNode.Features[j];
                    var id = StateFeaturesIdsService.GetStateId(feature.GetType(), (EcsStatesIds)stateNode.Id);
                    
                    runSystems.AddGroup(id, false, null, feature);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InjectAndInitSystems(List<object> injectParameters)
        {
            var injectArray = injectParameters.ToArray();
            foreach (var system in _systems)
            {
                system.Value.Inject(injectArray);
            }
            
            foreach (var systems in _systems)
            {
                systems.Value.Init();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            var systemsList = _systems.Values.ToList();
            for (var i = 0; i < systemsList.Count; i++)
            {
                systemsList[i].Destroy();
                systemsList[i] = null;
            }
            
            systemsList.Clear();
            _systems.Clear();
            
            _ecsWorld.Destroy();
            _ecsWorld = null;
        }
    }
}