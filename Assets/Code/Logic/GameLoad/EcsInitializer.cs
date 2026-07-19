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
        private readonly Dictionary<int, EcsSystems> _systems = new();
        private int _featuresSystemsId;
        
        private EcsWorld _ecsWorld;

        public EcsInitializer(
            List<object> injectParameters, 
            RuntimeEcsStateMachineGraph ecsStateMachineGraph, 
            EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;

            FillSystemsTypes();
            
            SelectSystems(ecsStateMachineGraph);
            
            InjectAndInitSystems(injectParameters);
        }

        private void FillSystemsTypes()
        {
            var systemTypes = Enum.GetValues(typeof(SystemType));
            foreach (var item in systemTypes)
            {
                _systems.Add((int)item, new EcsSystems(_ecsWorld));
            }
            
            // Separate EcsSystems for systems in features is need to make LeoEcsLite DI work. 
            // At the same time I don't want to fill SystemType with internal values that shouldn't be used by user.  
            _featuresSystemsId = systemTypes.Cast<int>().Max() + 1;
            _systems.Add(_featuresSystemsId, new EcsSystems(_ecsWorld));
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
            foreach (var (_, stateNode) in ecsConfig.AllRuntimeStateNodes)
            {
                // Select state change systems
                var stateChangeSystems = _systems[(int)SystemType.StateChangeSystems];
                for (var j = 0; j < stateNode.OnStateEnterSystems.Count; j++)
                {
                    stateChangeSystems.Add(stateNode.OnStateEnterSystems[j]);
                }
                for (var j = 0; j < stateNode.OnStateExitSystems.Count; j++)
                {
                    stateChangeSystems.Add(stateNode.OnStateEnterSystems[j]);
                }
                
                // Select system groups from features
                var runSystems = _systems[(int)SystemType.Run];
                var allFeaturesSystems = _systems[_featuresSystemsId];
                for (var j = 0; j < stateNode.Features.Count; j++)
                {
                    // This check is needed just in case the generation or id calculation went wrong. 
                    if (!Enum.IsDefined(typeof(EcsStatesIds), stateNode.Id))
                    {
                        throw new Exception($"Unknown state id: {stateNode.Id}");
                    }
                    
                    var feature = stateNode.Features[j];
                    var id = StateFeaturesIdsService.GetStateId(feature.GetType(), (EcsStatesIds)stateNode.Id);

                    var featureSystems = feature.GetSystems();
                    for (var i = 0; i < featureSystems.Length; i++)
                    {
                        allFeaturesSystems.Add(featureSystems[i]);
                    }
                    
                    runSystems.AddGroup(id, false, null, featureSystems);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InjectAndInitSystems(List<object> injectParameters)
        {
            var injectArray = injectParameters.ToArray();
            
            foreach (var systems in _systems)
            {
                systems.Value.Inject(injectArray);
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