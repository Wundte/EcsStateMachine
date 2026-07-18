using Code.Logic.Ecs.Features;

namespace Code.Demo.Logic.InputCapture
{
    public sealed class InputCaptureFeature : EcsFeature
    {
        public override void Init()
        {
            _systems.Add(new StateChangeInputCaptureSystem());
        }
    }
}