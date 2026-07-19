using Code.Logic.Ecs.Features;
using Leopotam.EcsLite;

namespace Code.Demo.Logic.CubeMovement.WhiteCubeMovement
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