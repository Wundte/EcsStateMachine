using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Code.EcsStateMachine.Runtime.Logic.Abstractions
{
    [System.Serializable]
    public abstract class EcsFeature
    {
        /// <summary>
        /// Override this method to add systems to feature
        /// </summary>
        [PublicAPI]
        public abstract IEcsSystem[] GetSystems();
    }
}