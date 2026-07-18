using Code.Logic.Ecs.Features;

namespace Code.Demo.Logic
{
    public class TestFeature2 : EcsFeature
    {
        public override void Init()
        {
            _systems.Add(new TEST_TWO());   
        }
    }
}