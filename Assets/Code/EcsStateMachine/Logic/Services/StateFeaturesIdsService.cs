using System;
using System.Collections.Generic;
using Code.EcsStateMachine.Data.EcsGraph;
using Generated;

namespace Code.EcsStateMachine.Logic.Services
{
    public sealed class StateFeaturesIdsService : IDisposable
    {
        /// <summary>
        /// Dictionary with ids for EcsFeatures belonging to certain state.
        /// </summary>
        private static readonly Dictionary<(Type, EcsStatesIds), string> Ids = new();

        public void Init(RuntimeEcsStateMachineGraph ecsConfig)
        {
            Ids.Clear();
            
            foreach (var (_, stateNode) in ecsConfig.AllRuntimeStateNodes)
            {
                var stateId = (EcsStatesIds)stateNode.Id;
                foreach (var feature in stateNode.Features)
                {
                    Ids.Add((feature.GetType(), stateId), $"ID_{feature.GetType().Name}_{stateId}");
                }
            }
        }

        public static string GetStateId(Type featureType, EcsStatesIds stateId)
        {
            if (Ids.TryGetValue((featureType, stateId), out var id))
            {
                return id;
            }

            throw new KeyNotFoundException($"Id for feature {featureType} in {stateId} state was not found.");
        }

        public void Dispose()
        {
            Ids.Clear();
        }
    }
}