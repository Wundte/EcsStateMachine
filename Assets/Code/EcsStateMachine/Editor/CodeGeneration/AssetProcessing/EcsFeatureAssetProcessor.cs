using Code.EcsStateMachine.Editor.CodeGeneration.Generation;
using Code.EcsStateMachine.Editor.CodeGeneration.Output;
using Code.EcsStateMachine.Editor.CodeGeneration.SourceGenerationData.Factories;
using Code.EcsStateMachine.Editor.CodeGeneration.SourceGenerationData.Utils;
using Code.EcsStateMachine.Editor.Settings;
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
    internal sealed class EcsFeatureAssetProcessor : AssetPostprocessor
    {
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
            var featureTypes = TypeFinder.FindDerivedTypes<EcsFeature>();

            const string enumName = "EcsFeatureIds";
            var enumDefinition = EnumSourceGenerator.CreateDefinition(
                featureTypes,
                enumName,
                EcsStateMachineSettings.instance.GeneratedNamespace);
            FileWriter.Write($"{enumName}.cs", EnumSourceGenerator.Generate(enumDefinition));

            const string factoryName = "EcsFeatureFactory";
            var factoryDefinition = new FactoryDefinition
            {
                Namespace = EcsStateMachineSettings.instance.GeneratedNamespace,
                FactoryName = factoryName,
                EnumName = enumName,
                ReturnType = typeof(EcsFeature),
                Types = featureTypes
            };
            FileWriter.Write($"{factoryName}.cs", FactorySourceGenerator.Generate(factoryDefinition));

            AssetDatabase.Refresh();

            Debug.Log("<color=#00C853>EcsFeatureIds and Factory successfully generated</color>");
        }
    }
}