using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace Code.EcsStateMachine.Editor.EcsStateMachineGraph
{
    /// <summary>
    /// Graph asset representing ECS state machine configuration.
    /// </summary>
    [System.Serializable]
    [Graph(AssetFileExtension)]
    public sealed class EcsStateMachineGraph : Graph
    {
        public const string AssetFileExtension = "ecsgraph";
        
        [MenuItem("Assets/Create/Configs/Ecs Graph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<EcsStateMachineGraph>();   
        }
    }
}