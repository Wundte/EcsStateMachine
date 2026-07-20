using Code.EcsStateMachine.Runtime.Logic.Abstractions;
using Leopotam.EcsLite;

namespace Code.Demo.Runtime.Logic.CubeMovement.BlueCubeMovement
{
    public sealed class BlueCubeMovementFeature : EcsFeature
    {
        public override IEcsSystem[] GetSystems()
        {
            return new IEcsSystem[]
            {
                new BlueCubeMovementSystem(),
            };
        }
    }
}