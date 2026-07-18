using System;
using System.Collections.Generic;
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
        // private static Dictionary<EcsStatesIds, RuntimeStateNode> _stateNodes = new();
        private static EcsWorld _ecsWorld;
        
        public void Init(RuntimeEcsStateMachineGraph ecsStateMachineGraph, EcsWorld ecsWorld)
        {
            _ecsStateMachineGraph = ecsStateMachineGraph;

            // foreach (var (_, stateNode) in ecsStateMachineGraph.AllRuntimeStateNodes)
            // {
            //     // We can safely parse because enum values are generated from state names.
            //     var stateId = Enum.Parse<EcsStatesIds>(stateNode.Name);
            //     _stateNodes.Add(stateId, stateNode);
            // }
            
            _ecsWorld = ecsWorld;
        }

        public static void NextState()
        {
            // var currentStateStateNode = _stateNodes[CurrentState];
            
        }

        public static void NextState(EcsStatesIds state)
        {
            
        }

        private static void ChangeState(EcsStatesIds oldState, EcsStatesIds newState)
        {
            
        }

        public static void DeactivateSystemsForState(EcsStatesIds state)
        {
            // if (_stateNodes.TryGetValue(state, out var runtimeStateNode))
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