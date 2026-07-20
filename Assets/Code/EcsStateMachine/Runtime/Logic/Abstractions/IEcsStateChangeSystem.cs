using Leopotam.EcsLite;

namespace Code.EcsStateMachine.Runtime.Logic.Abstractions
{
    /// <summary>
    /// ECS system executed during state transitions.
    /// </summary>
    public interface IEcsStateChangeSystem : IEcsRunSystem
    {
        /// <summary>
        /// Executes state change logic.
        /// </summary>
        void Run();
        
        /// <summary>
        /// Explicit implementation of ECS run method.
        /// Allows state change systems to use parameterless Run method
        /// while keeping compatibility with IEcsRunSystem.
        /// </summary>
        void IEcsRunSystem.Run(EcsSystems systems)
        {
            Run();
        }
    }
}