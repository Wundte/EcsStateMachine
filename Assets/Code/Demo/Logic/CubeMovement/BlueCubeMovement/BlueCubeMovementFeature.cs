using Code.Logic.Ecs.Features;
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