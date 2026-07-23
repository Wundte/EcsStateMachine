using Code.EcsStateMachine.Editor.CodeGeneration.Generation.Factories;
using Code.EcsStateMachine.Editor.CodeGeneration.Generation.Types;
using Code.EcsStateMachine.Editor.CodeGeneration.Output;
using Code.EcsStateMachine.Editor.CodeGeneration.SourceGenerationData.Factories;
using Code.EcsStateMachine.Editor.CodeGeneration.SourceGenerationData.Utils;
using Code.EcsStateMachine.Runtime.Logic.Abstractions;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEngine;

namespace Code.EcsStateMachine.Editor.CodeGeneration.AssetProcessing
{
    /// <summary>
    /// Unity editor asset processor responsible for generating source code
    /// for ECS state change systems.
    ///
    /// Generates:
    /// - EcsStateChangeSystemsIds enum containing identifiers for all systems
    ///   implementing <see cref="IEcsStateChangeSystem"/>.
    /// - Factory class responsible for creating state change system instances
    ///   from generated identifiers.
    ///
    /// The processor runs only inside the Unity Editor and is excluded from runtime builds.
    /// </summary>
    public class EcsStateChangeSystemsAssetProcessor : AssetPostprocessor
    {
        /// <summary>
        /// Called automatically by Unity after scripts have been recompiled.
        ///
        /// Keeps generated state change system files synchronized with the
        /// current set of available systems.
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Generate();
        }

        /// <summary>
        /// Generates source files required for ECS state change system registration.
        ///
        /// Generation pipeline:
        /// 1. Finds all types implementing <see cref="IEcsStateChangeSystem"/>.
        /// 2. Creates an enum containing stable identifiers for discovered systems.
        /// 3. Generates a factory that maps identifiers to concrete system instances.
        /// 4. Writes generated source code to project files.
        /// 5. Refreshes Unity asset database to trigger recompilation.
        ///
        /// Can also be executed manually through:
        /// Tools → Generate ECS Systems
        /// </summary>
        [MenuItem("Tools/Generate ECS Systems")]
        public static void Generate()
        {
            var systemTypes = TypeFinder.FindDerivedTypes<IEcsStateChangeSystem>();

            var enumDefinition = EnumSourceGenerator.CreateDefinition(
                systemTypes,
                "EcsStateChangeSystemsIds",
                "Code.EcsStateMachine.Runtime.Generated");
            FileWriter.Write("EcsStateChangeSystemsIds.cs", EnumSourceGenerator.Generate(enumDefinition));

            var factoryDefinition = new FactoryDefinition
            {
                Namespace = "Code.EcsStateMachine.Runtime.Generated",
                FactoryName = "EcsStateChangeSystemsFactory",
                EnumName = "EcsStateChangeSystemsIds",
                ReturnType = typeof(IEcsStateChangeSystem),
                Types = systemTypes
            };
            FileWriter.Write("EcsStateChangeSystemsFactory.cs", FactorySourceGenerator.Generate(factoryDefinition));

            AssetDatabase.Refresh();

            Debug.Log("<color=#00C853>EcsOnStateChangeSystemsIds and Factory successfully generated</color>");
        }
    }
}