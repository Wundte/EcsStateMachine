using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Logic.GameLoad
{
    [DisallowMultipleComponent] [DefaultExecutionOrder(int.MinValue)] [HideMonoScript]
    public sealed class EntryPoint : MonoBehaviour
    {
        private async void Awake()
        {
            // Load scenes (if needed)
            // Load configs
            // Load services (if any) // services as injected non-static classes
            // Load pools
            
            // Load ecs
            // var ecsInitializer = new EcsInitializer();
        }
    }
}
