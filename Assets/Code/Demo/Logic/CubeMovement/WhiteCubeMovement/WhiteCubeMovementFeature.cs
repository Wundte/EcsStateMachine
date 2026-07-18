using Code.Logic.Ecs.Features;

namespace Code.Demo.Logic.CubeMovement.WhiteCubeMovement
{
    public sealed class WhiteCubeMovementFeature : EcsFeature
    {
        public override void Init()
        {
            _systems.Add(new WhiteCubeMovementSystem());
        }
    }
}