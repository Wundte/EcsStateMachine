using UnityEditor;

namespace Code.EcsStateMachine.Editor.Settings
{
    public sealed class EcsStateMachineSettings : ScriptableSingleton<EcsStateMachineSettings>
    {
        public string GeneratedNamespace = "Code.EcsStateMachine.Runtime.Generated";

        public void Save()
        {
            Save(true);
        }
    }
}