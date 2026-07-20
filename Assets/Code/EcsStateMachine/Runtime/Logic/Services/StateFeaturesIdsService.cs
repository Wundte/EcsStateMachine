using System;
using System.Collections.Generic;
using Code.EcsStateMachine.Runtime.Data.EcsGraph;
using Generated;

namespace Code.EcsStateMachine.Runtime.Logic.Services
{
    /// <summary>
    /// Provides identifiers for feature system groups.
    /// </summary>
    public sealed class StateFeaturesIdsService : IDisposable
    {
        /// <summary>
        /// Stores identifiers for feature instances grouped by ECS state.
        /// Key contains feature type and state identifier.
        /// </summary>
        private static readonly Dictionary<(Type, EcsStatesIds), string> Ids = new();

        /// <summary>
        /// Generates identifiers for all features defined in state machine graph.
        /// </summary>
        public void Init(RuntimeEcsStateMachineGraph ecsConfig)
        {
            Ids.Clear();
            
            foreach (var (_, stateNode) in ecsConfig.AllRuntimeStateNodes)
            {
                var stateId = (EcsStatesIds)stateNode.Id;

                foreach (var feature in stateNode.Features)
                {
                    Ids.TryAdd(
                        (feature.GetType(), stateId),
                        $"ID_{feature.GetType().Name}_{stateId}");
                }
            }
        }

        /// <summary>
        /// Returns system group identifier for feature in specified state.
        /// </summary>
        public static string GetStateId(Type featureType, EcsStatesIds stateId)
        {
            if (Ids.TryGetValue((featureType, stateId), out var id))
            {
                return id;
            }

            throw new KeyNotFoundException($"Id for feature {featureType} in {stateId} state was not found.");
        }

        /// <summary>
        /// Clears cached feature identifiers.
        /// </summary>
        public void Dispose()
        {
            Ids.Clear();
        }
    }
}