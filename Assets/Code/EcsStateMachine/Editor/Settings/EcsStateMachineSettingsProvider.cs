using UnityEditor;

namespace Code.EcsStateMachine.Editor.Settings
{
    public static class EcsStateMachineSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider("Project/ECS State Machine", SettingsScope.Project)
            {
                label = "ECS State Machine",
                guiHandler = _ =>
                {
                    var settings = EcsStateMachineSettings.instance;
                    
                    EditorGUI.BeginChangeCheck();

                    settings.GeneratedNamespace = EditorGUILayout.TextField(
                        "Generated Namespace", 
                        settings.GeneratedNamespace);

                    if (EditorGUI.EndChangeCheck())
                    {
                        settings.Save();
                    }
                }
            };
        }
    }
}