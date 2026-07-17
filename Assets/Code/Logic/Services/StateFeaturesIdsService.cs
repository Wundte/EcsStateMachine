using System;
using System.Collections.Generic;
using Code.Data.Ecs.EcsStateMachine;
using Generated;

namespace Code.Logic.Services
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
                // We can safely parse because enum values are generated from state names.
                var stateId = Enum.Parse<EcsStatesIds>(stateNode.Name);
                foreach (var feature in stateNode.Features)
                {
                    Ids.Add((feature.GetType(), stateId), $"ID_{feature.GetType().Name}_{stateNode.Name}");
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