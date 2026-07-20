using System;
using Code.EcsStateMachine.Runtime.Data.EcsGraph;
using Cysharp.Threading.Tasks;
using Generated;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Code.EcsStateMachine.Runtime.Logic.Services
{
    /// <summary>
    /// Controls ECS state machine transitions and feature activation.
    /// </summary>
    public sealed class EcsStateService : IDisposable
    {
        /// <summary>
        /// Current active ECS state.
        /// </summary>
        public static EcsStatesIds CurrentState { get; private set; } = EcsStatesIds.None;
        
        private static RuntimeEcsStateMachineGraph _ecsStateMachineGraph;
        private static EcsWorld _ecsWorld;
        
        /// <summary>
        /// Initializes state service dependencies.
        /// </summary>
        public void Init(RuntimeEcsStateMachineGraph ecsStateMachineGraph, EcsWorld ecsWorld)
        {
            _ecsStateMachineGraph = ecsStateMachineGraph;
            _ecsWorld = ecsWorld;
        }

        /// <summary>
        /// Activates default state from ECS state machine graph.
        /// </summary>
        public static void SetInitialState()
        {
            ChangeState(CurrentState, _ecsStateMachineGraph.DefaultState);
        }
        
        /// <summary>
        /// Changes state using current state's default transition.
        /// </summary>
        public static void NextState()
        {
            var currentStateStateNode = _ecsStateMachineGraph.AllRuntimeStateNodes[(int)CurrentState];
            var defaultNextState = (EcsStatesIds)currentStateStateNode.DefaultNextState;
            
            ChangeState(CurrentState, defaultNextState);
        }

        /// <summary>
        /// Changes state if transition is allowed by graph configuration.
        /// </summary>
        public static void NextState(EcsStatesIds state)
        {
            var currentStateStateNode = _ecsStateMachineGraph.AllRuntimeStateNodes[(int)CurrentState];

            if (currentStateStateNode.PossibleNextStates.Contains((int)state))
            {
                ChangeState(CurrentState, state);
            }
        }

        /// <summary>
        /// Performs state transition sequence:
        /// deactivate old features, execute exit systems,
        /// activate new features and execute enter systems.
        /// </summary>
        private static async UniTask ChangeState(EcsStatesIds oldState, EcsStatesIds newState)
        {
            if (oldState == newState)
            {
                return;
            }
            
            if (_ecsStateMachineGraph.AllRuntimeStateNodes.TryGetValue((int)oldState, out var oldStateNode))
            {
                // Deactivate features of previous state.
                for (var i = 0; i < oldStateNode.Features.Count; i++)
                {
                    var featureType = oldStateNode.Features[i].GetType();
                    
                    ChangeSystemGroupState(featureType, oldState, false);
                }
                
                // Execute exit systems.
                for (var i = 0; i < oldStateNode.OnStateExitSystems.Count; i++)
                {
                    oldStateNode.OnStateExitSystems[i].Run();
                }
            }

            await UniTask.DelayFrame(1);
            
            if (_ecsStateMachineGraph.AllRuntimeStateNodes.TryGetValue((int)newState, out var newStateNode))
            {
                // Execute enter systems.
                for (var i = 0; i < newStateNode.OnStateEnterSystems.Count; i++)
                {
                    newStateNode.OnStateEnterSystems[i].Run();
                }
                
                // Activate features of new state.
                for (var i = 0; i < newStateNode.Features.Count; i++)
                {
                    var featureType = newStateNode.Features[i].GetType();
                    
                    ChangeSystemGroupState(featureType, newState, true);
                }
            }
            
            CurrentState = newState;
            
#if UNITY_EDITOR
            Debug.Log($"<color=#00C853>Ecs State changed from {oldState} to {newState}</color>");
#endif
        }

        /// <summary>
        /// Sends event to change feature system group activation state.
        /// </summary>
        private static void ChangeSystemGroupState(Type featureType, EcsStatesIds state, bool newState)
        {
            var entity = _ecsWorld.NewEntity();
            var groupName = StateFeaturesIdsService.GetStateId(featureType, state);
            
            ref var eventGroup = ref _ecsWorld.GetPool<EcsGroupSystemState>().Add(entity);
            eventGroup.Name = groupName;
            eventGroup.State = newState;
        }
        
        /// <summary>
        /// Releases references to state machine data.
        /// </summary>
        public void Dispose()
        {
            _ecsStateMachineGraph = null;
        }
    }
}