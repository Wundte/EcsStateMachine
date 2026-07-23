using System;
using System.Collections.Generic;

namespace Code.EcsStateMachine.Editor.CodeGeneration.SourceGenerationData.Factories
{
    /// <summary>
    /// Contains data required to generate a factory.
    /// </summary>
    public sealed class FactoryDefinition
    {
        public string Namespace;
        public string FactoryName;
        public string EnumName;

        public Type ReturnType;

        public IReadOnlyList<Type> Types;
    }
}