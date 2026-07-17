using System.Collections.Generic;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Code.Logic.Ecs.Features
{
    public abstract class EcsFeature
    {
        protected readonly List<IEcsSystem> _systems = new();
        
        /// <summary>
        /// Override this method to add systems to feature
        /// </summary>
        [PublicAPI]
        public abstract void Init();
        
        public static implicit operator IEcsSystem[](EcsFeature feature)
        {
            feature._systems.Clear();
            
            feature.Init();
            
            return feature._systems.ToArray();
        }
    }
}