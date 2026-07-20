using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Code.EcsStateMachine.Runtime.Data.Constants;
using Code.EcsStateMachine.Runtime.Data.EcsGraph;
using Code.EcsStateMachine.Runtime.Logic.Services;
using Generated;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Code.EcsStateMachine.Runtime.Logic.Initialization
{
    /// <summary>
    /// Initializes ECS world and configures system execution groups.
    /// </summary>
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

        /// <summary>
        /// Creates ECS system groups.
        /// </summary>
        private void FillSystemsTypes()
        {
            var systemTypes = Enum.GetValues(typeof(EcsSystemType));
            foreach (var item in systemTypes)
            {
                _systems.Add((int)item, new EcsSystems(_ecsWorld));
            }
            
            // Separate system group for feature systems.
            _featuresSystemsId = systemTypes.Cast<int>().Max() + 1;
            _systems.Add(_featuresSystemsId, new EcsSystems(_ecsWorld));
        }
        
        /// <summary>
        /// Creates Unity update loop and starts initial state.
        /// </summary>
        public void StartEcsLoop()
        {
            var ecsLoopGameObject = new GameObject
            {
#if UNITY_EDITOR
                name = "EcsLoop"
#endif
            };
            var ecsLoop = ecsLoopGameObject.AddComponent<EcsLoop.EcsLoop>();

            ecsLoop.Init(_systems);
            
            EcsStateService.SetInitialState();
        }

        /// <summary>
        /// Selects systems and features from state graph configuration.
        /// </summary>
        private void SelectSystems(RuntimeEcsStateMachineGraph ecsConfig)
        {
            foreach (var (_, stateNode) in ecsConfig.AllRuntimeStateNodes)
            {
                // Select state change systems
                var stateChangeSystems = _systems[(int)EcsSystemType.StateChangeSystems];
                for (var j = 0; j < stateNode.OnStateEnterSystems.Count; j++)
                {
                    stateChangeSystems.Add(stateNode.OnStateEnterSystems[j]);
                }
                for (var j = 0; j < stateNode.OnStateExitSystems.Count; j++)
                {
                    stateChangeSystems.Add(stateNode.OnStateExitSystems[j]);
                }
                
                // Select system groups from features
                var runSystems = _systems[(int)EcsSystemType.Run];
                var allFeaturesSystems = _systems[_featuresSystemsId];

                for (var j = 0; j < stateNode.Features.Count; j++)
                {
                    // Validate generated state identifier.
                    if (!Enum.IsDefined(typeof(EcsStatesIds), stateNode.Id))
                    {
                        throw new Exception($"Unknown state id: {stateNode.Id}");
                    }
                    
                    var feature = stateNode.Features[j];
                    var featureSystems = feature.GetSystems();

                    if (featureSystems is not null && featureSystems.Length > 0)
                    {
                        var id = StateFeaturesIdsService.GetStateId(feature.GetType(), (EcsStatesIds)stateNode.Id);

                        for (var i = 0; i < featureSystems.Length; i++)
                        {
                            allFeaturesSystems.Add(featureSystems[i]);
                        }

                        runSystems.AddGroup(id, false, null, featureSystems);
                    }
                    else
                    {
                        Debug.LogError($"Systems list cant be null or empty for {feature.GetType()}");
                    }
                }
            }
        }
        
        /// <summary>
        /// Injects dependencies and initializes all ECS systems.
        /// </summary>
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

        /// <summary>
        /// Destroys ECS systems and world.
        /// </summary>
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