using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Code.Logic.Ecs.Editor.SystemsTypesImporter
{
    public class EnumGeneratorPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            bool scriptsChanged = importedAssets.Any(x => x.EndsWith(".cs")) 
                                  || deletedAssets.Any(x => x.EndsWith(".cs")) 
                                  || movedAssets.Any(x => x.EndsWith(".cs"));

            if (!scriptsChanged)
            {
                return;
            }

            Debug.Log($"CALLING");
            
            // AbilityEnumGenerator.Generate();
        }
    }
}