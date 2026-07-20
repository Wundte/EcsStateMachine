using Code.EcsStateMachine.Editor.CodeGeneration.Generation;
using Code.EcsStateMachine.Editor.CodeGeneration.Generation.Factories;
using Code.EcsStateMachine.Editor.CodeGeneration.Output;
using Code.EcsStateMachine.Runtime.Logic.Abstractions;
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
        /// Shared code generator configuration used for creating the generated
        /// state change systems identifier enum.
        /// </summary>
        private static readonly EcsCodeGenerator Generator = new (
            enumName: "EcsStateChangeSystemsIds",
            enumNamespace: "Code.EcsStateMachine.Runtime.Generated",
            outputFileName: "EcsStateChangeSystemsIds.cs");

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
            var systemTypes = EcsCodeGenerator.FindTypesDerivedFrom<IEcsStateChangeSystem>();

            var enumDefinition = Generator.CreateEnumDefinition(systemTypes);
            Generator.WriteEnum(enumDefinition);

            var systemTypeMap = EcsCodeGenerator.CreateTypeMap(systemTypes);
            var factoryCode = EcsStateChangeSystemsFactoryGenerator.GenerateFactory(systemTypeMap);

            FileWriter.Write("EcsRunSystemsFactory.cs", factoryCode);

            AssetDatabase.Refresh();

            Debug.Log("<color=#00C853>EcsOnStateChangeSystemsIds and Factory successfully generated</color>");
        }
    }
}