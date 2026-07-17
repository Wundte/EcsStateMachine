using System;
using Code.Data.Ecs.EcsStateMachine;
using Generated;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;

namespace Code.Logic.Services
{
    public sealed class GameStateService : IDisposable
    {
        public static EcsStatesIds CurrentState { get; private set; }
        
        private static RuntimeEcsStateMachineGraph _ecsStateMachineGraph;
        private static EcsWorld _ecsWorld;
        
        public void Init(RuntimeEcsStateMachineGraph ecsStateMachineGraph, EcsWorld ecsWorld)
        {
            _ecsStateMachineGraph = ecsStateMachineGraph;
            _ecsWorld = ecsWorld;
        }

        public static void NextState()
        {
            
        }

        public static void NextState(EcsStatesIds state)
        {
            
        }

        public static void DeactivateSystemsForState(EcsStatesIds state)
        {
            // if (_ecsStateMachineGraph.AllRuntimeStateNodes.TryGetValue(state, out var runtimeStateNode))
            // {
            //     
            // }
            //
            // var featureGroup = _featureGroupContainers[state];
            //
            // foreach (var featureContainer in featureGroup.Features)
            // {
            //     var featureType = featureContainer.Feature.GetType();
            //     ChangeSystemGroupState(featureType, featureGroup.GameState, false);
            // }
        }

        private static void ChangeSystemGroupState(Type featureType, EcsStatesIds state, bool newState)
        {
            var entity = _ecsWorld.NewEntity();
            var groupName = StateFeaturesIdsService.GetStateId(featureType, state);
            
            ref var eventGroup = ref _ecsWorld.GetPool<EcsGroupSystemState>().Add(entity);
            eventGroup.Name = groupName;
            eventGroup.State = newState;
        }
        
        public void Dispose()
        {
            _ecsStateMachineGraph = null;
        }
    }
}