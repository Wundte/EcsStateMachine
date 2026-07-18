using Code.Logic.Ecs.Features;

namespace Code.Demo.Logic.CubeMovement.BlueCubeMovement
{
    public sealed class BlueCubeMovementFeature : EcsFeature
    {
        public override void Init()
        {
            _systems.Add(new BlueCubeMovementSystem());
        }
    }
}