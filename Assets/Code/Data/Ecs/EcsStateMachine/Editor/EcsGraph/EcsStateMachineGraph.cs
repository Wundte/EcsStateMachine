using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace Code.Data.Ecs.EcsStateMachine.Editor.EcsGraph
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