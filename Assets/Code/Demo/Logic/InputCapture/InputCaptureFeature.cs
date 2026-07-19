using Code.Logic.Ecs.Features;
using Leopotam.EcsLite;

namespace Code.Demo.Logic.InputCapture
{
    public sealed class InputCaptureFeature : EcsFeature
    {
        public override IEcsSystem[] GetSystems()
        {
            return new IEcsSystem[]
            {
                new StateChangeInputCaptureSystem(),
            };
        }
    }
}