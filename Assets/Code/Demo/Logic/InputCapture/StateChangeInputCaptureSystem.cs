using Code.Logic.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Demo.Logic.InputCapture
{
    public sealed class StateChangeInputCaptureSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameStateService.NextState();
            }
        }
    }
}