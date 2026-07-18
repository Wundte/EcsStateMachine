using System;
using Code.Data.Ecs.EcsStateMachine;
using Cysharp.Threading.Tasks;
using Generated;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Code.Logic.Services
{
    public sealed class GameStateService : IDisposable
    {
        public static EcsStatesIds CurrentState { get; private set; } = EcsStatesIds.None;
        
        private static RuntimeEcsStateMachineGraph _ecsStateMachineGraph;
        private static EcsWorld _ecsWorld;
        
        public void Init(RuntimeEcsStateMachineGraph ecsStateMachineGraph, EcsWorld ecsWorld)
        {
            _ecsStateMachineGraph = ecsStateMachineGraph;
            _ecsWorld = ecsWorld;
        }

        public static void SetInitialState()
        {
            ChangeState(CurrentState, _ecsStateMachineGraph.DefaulState);
        }
        
        public static void NextState()
        {
            var currentStateStateNode = _ecsStateMachineGraph.AllRuntimeStateNodes[(int)CurrentState];
            var defaultNextState = (EcsStatesIds)currentStateStateNode.DefaultNextState;
            
            ChangeState(CurrentState, defaultNextState);
        }

        public static void NextState(EcsStatesIds state)
        {
            var currentStateStateNode = _ecsStateMachineGraph.AllRuntimeStateNodes[(int)CurrentState];
            if (currentStateStateNode.PossibleNextStates.Contains((int)state))
            {
                ChangeState(CurrentState, state);
            }
        }

        private static async void ChangeState(EcsStatesIds oldState, EcsStatesIds newState)
        {
            Debug.Log($"Changing state from {oldState} to {newState}");
            
            if (oldState == newState)
            {
                return;
            }
            
            if (_ecsStateMachineGraph.AllRuntimeStateNodes.TryGetValue((int)oldState, out var oldStateNode))
            {
                // Deactivate features for old state
                for (var i = 0; i < oldStateNode.Features.Count; i++)
                {
                    var featureType  = oldStateNode.Features[i].GetType();
                    
                    ChangeSystemGroupState(featureType, oldState, false);
                }
                
                // Run on state exit systems
                for (var i = 0; i < oldStateNode.OnStateExitSystems.Count; i++)
                {
                    oldStateNode.OnStateExitSystems[i].Run();
                }
            }

            await UniTask.DelayFrame(1);
            
            if (_ecsStateMachineGraph.AllRuntimeStateNodes.TryGetValue((int)newState, out var newStateNode))
            {
                // Run on state enter systems
                for (var i = 0; i < newStateNode.OnStateExitSystems.Count; i++)
                {
                    newStateNode.OnStateEnterSystems[i].Run();
                }
                
                // Activate features for old state
                for (var i = 0; i < newStateNode.Features.Count; i++)
                {
                    var featureType  = newStateNode.Features[i].GetType();
                    
                    ChangeSystemGroupState(featureType, newState, true);
                }
            }
            
            CurrentState = newState;
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