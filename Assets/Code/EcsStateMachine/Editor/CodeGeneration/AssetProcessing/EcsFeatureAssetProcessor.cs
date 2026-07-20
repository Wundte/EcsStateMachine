using Code.EcsStateMachine.Editor.CodeGeneration.Generation;
using Code.EcsStateMachine.Editor.CodeGeneration.Generation.Factories;
using Code.EcsStateMachine.Editor.CodeGeneration.Output;
using Code.EcsStateMachine.Runtime.Logic.Abstractions;
using UnityEditor;
using UnityEngine;

namespace Code.EcsStateMachine.Editor.CodeGeneration.AssetProcessing
{
    /// <summary>
    /// Unity editor asset processor responsible for generating ECS State Machine
    /// related source code after script recompilation or manual execution.
    ///
    /// Generates:
    /// - EcsFeatureIds enum containing identifiers for all available ECS features.
    /// - EcsFeatureFactory used to create feature instances without manual registration.
    ///
    /// This class is executed only in the Unity Editor and does not affect runtime builds.
    /// </summary>
    public class EcsFeatureAssetProcessor : AssetPostprocessor
    {
        /// <summary>
        /// Shared code generator configuration for ECS feature related generated files.
        /// </summary>
        private static readonly EcsCodeGenerator Generator = new EcsCodeGenerator(
            enumName: "EcsFeatureIds",
            enumNamespace: "Code.EcsStateMachine.Runtime.Generated",
            outputFileName: "EcsFeatureIds.cs");

        /// <summary>
        /// Called automatically by Unity after scripts are recompiled.
        ///
        /// Regenerates generated ECS feature files to keep them synchronized
        /// with the current project state.
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Generate();
        }

        /// <summary>
        /// Generates ECS feature related source files.
        ///
        /// The generation pipeline:
        /// 1. Finds all types derived from <see cref="EcsFeature"/>.
        /// 2. Creates an enum containing feature identifiers.
        /// 3. Generates a factory mapping identifiers to concrete feature types.
        /// 4. Writes generated files to disk.
        /// 5. Refreshes Unity asset database to trigger recompilation.
        ///
        /// Can be executed manually through:
        /// Tools → Generate ECS Features
        /// </summary>
        [MenuItem("Tools/Generate ECS Features")]
        public static void Generate()
        {
            var featureTypes = EcsCodeGenerator.FindTypesDerivedFrom<EcsFeature>();

            var enumDefinition = Generator.CreateEnumDefinition(featureTypes);
            Generator.WriteEnum(enumDefinition);

            var featureTypeMap = EcsCodeGenerator.CreateTypeMap(featureTypes);
            var factoryCode = EcsFeatureFactoryGenerator.GenerateFactory(featureTypeMap);

            FileWriter.Write("EcsFeatureFactory.cs", factoryCode);

            AssetDatabase.Refresh();

            Debug.Log("<color=#00C853>EcsFeatureIds and Factory successfully generated</color>");
        }
    }
}