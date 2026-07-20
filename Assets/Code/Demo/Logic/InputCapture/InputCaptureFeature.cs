using Code.EcsStateMachine.Logic.Abstractions;
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