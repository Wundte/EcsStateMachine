using Code.EcsStateMachine.Logic.Abstractions;
using Leopotam.EcsLite;

namespace Code.Demo.Logic.CubeMovement.GreenCubeMovement
{
    public sealed class GreenCubeMovementFeature : EcsFeature
    {
        public override IEcsSystem[] GetSystems()
        {
            return new IEcsSystem[]
            {
                new GreenCubeMovementSystem(),
            };
        }
    }
}