using Code.EcsStateMachine.Runtime.Logic.Abstractions;
using Leopotam.EcsLite;

namespace Code.Demo.Runtime.Logic.CubeMovement.GreenCubeMovement
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