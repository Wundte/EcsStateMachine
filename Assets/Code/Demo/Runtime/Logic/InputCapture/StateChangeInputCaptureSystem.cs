using Code.EcsStateMachine.Runtime.Logic.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Demo.Runtime.Logic.InputCapture
{
    public sealed class StateChangeInputCaptureSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EcsStateService.NextState();
            }
        }
    }
}