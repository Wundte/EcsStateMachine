using Code.EcsStateMachine.Logic.Abstractions;
using Leopotam.EcsLite;

namespace Code.Demo.Logic.CubeMovement.BlueCubeMovement
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