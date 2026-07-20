using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Code.EcsStateMachine.Runtime.Logic.Abstractions
{
    /// <summary>
    /// Base class for ECS state features.
    /// A feature is a group of systems that are executed together after activation.
    /// </summary>
    [System.Serializable]
    public abstract class EcsFeature
    {
        /// <summary>
        /// Returns systems included in this feature.
        /// Systems returned by this method will be added to ECS schedule after feature activation.
        /// </summary>
        [PublicAPI]
        public abstract IEcsSystem[] GetSystems();
    }
}