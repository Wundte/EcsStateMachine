using Leopotam.EcsLite;

namespace Code.EcsStateMachine.Runtime.Logic.Abstractions
{
    public interface IEcsStateChangeSystem : IEcsRunSystem
    {
        void Run();
        
        // Avoid having to implement this method in all IEcsStateChangeSystem implementers. 
        // Run() and Run(EcsSystems) are duplicates, since everything we can get from system we can also get from DI
        void IEcsRunSystem.Run(EcsSystems systems)
        {
            Run();
        }
    }
}