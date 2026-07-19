using Code.Logic.Ecs.Features;
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