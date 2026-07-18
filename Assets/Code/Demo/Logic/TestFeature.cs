using Code.Logic.Ecs.Features;

namespace Code.Logic.Game
{
    public class TestFeature : EcsFeature
    {
        public override void Init()
        {
            _systems.Add(new TEST_ONE());   
        }
    }
}