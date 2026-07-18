using Code.Logic.Ecs.Features;

namespace Code.Demo.Logic.CubeMovement.GreenCubeMovement
{
    public sealed class GreenCubeMovementFeature : EcsFeature
    {
        public override void Init()
        {
            _systems.Add(new GreenCubeMovementSystem());
        }
    }
}