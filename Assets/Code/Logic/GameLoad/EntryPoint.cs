using System;
using System.Collections.Generic;
using Code.Data.Ecs.EcsStateMachine;
using Code.Logic.Services;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Logic.GameLoad
{
    [DisallowMultipleComponent] [DefaultExecutionOrder(int.MinValue)] [HideMonoScript]
    public sealed class EntryPoint : MonoBehaviour
    {
        private readonly List<IDisposable> _services = new();

        private async void Awake()
        {
            try
            {
                var ecsWorld = new EcsWorld();
                
                var configs = await LoadConfigs();
                var ecsStateMachineGraph = configs[typeof(RuntimeEcsStateMachineGraph)] as RuntimeEcsStateMachineGraph;
                
                // Load resources
                
                await CreateServices(ecsStateMachineGraph, ecsWorld);
        
                // Load scenes (if needed)
                
                // Load pools
                
                var injectParameters = CreateInjectParameters(configs);
                
                // Load ECS
                var ecsInitializer = new EcsInitializer(injectParameters, ecsStateMachineGraph, ecsWorld);
                
                ecsInitializer.StartEcsLoop();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private async UniTask<Dictionary<Type, ScriptableObject>> LoadConfigs()
        {
            var configsLoaderService = new ConfigsLoaderService();
            _services.Add(configsLoaderService);
            
            return await configsLoaderService.Init();
        }
        
        private async UniTask CreateServices(RuntimeEcsStateMachineGraph ecsStateMachineGraph, EcsWorld ecsWorld)
        {
            var stateFeatureIdsService = new StateFeaturesIdsService();
            stateFeatureIdsService.Init(ecsStateMachineGraph);
            _services.Add(stateFeatureIdsService);

            var gameStateService = new GameStateService();
            gameStateService.Init(ecsStateMachineGraph, ecsWorld);
            _services.Add(gameStateService);
        }

        private List<object> CreateInjectParameters(Dictionary<Type, ScriptableObject> configs)
        {
            var injectParameters = new List<object>();
            foreach (var config in configs)
            {
                injectParameters.Add(config);
            }
            injectParameters.AddRange(_services);
            
            return injectParameters;
        }
        
        private void OnDestroy()
        {
            for (var i = _services.Count - 1; i >= 0; i--)
            {
                _services[i]?.Dispose();
            }
        
            _services.Clear();
        }
    }
}
