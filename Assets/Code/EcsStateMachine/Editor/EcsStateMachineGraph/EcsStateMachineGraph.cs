using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace Code.EcsStateMachine.Editor.EcsStateMachineGraph
{
    [System.Serializable]
    [Graph(AssetFileExtension)]
    public class EcsStateMachineGraph : Graph
    {
        public const string AssetFileExtension = "ecsgraph";
        
        [MenuItem("Assets/Create/Configs/Ecs Graph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<EcsStateMachineGraph>();   
        }
    }
}