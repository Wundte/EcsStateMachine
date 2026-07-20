using Code.EcsStateMachine.Runtime.Logic.Abstractions;
using Leopotam.EcsLite;

namespace Code.Demo.Runtime.Logic.InputCapture
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