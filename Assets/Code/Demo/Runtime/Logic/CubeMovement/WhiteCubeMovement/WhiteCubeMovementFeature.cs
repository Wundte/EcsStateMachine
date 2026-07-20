using Code.EcsStateMachine.Runtime.Logic.Abstractions;
using Leopotam.EcsLite;

namespace Code.Demo.Runtime.Logic.CubeMovement.WhiteCubeMovement
{
    public sealed class WhiteCubeMovementFeature : EcsFeature
    {
        public override IEcsSystem[] GetSystems()
        {
            return new IEcsSystem[]
            {
                new WhiteCubeMovementSystem(),
            };
        }
    }
}