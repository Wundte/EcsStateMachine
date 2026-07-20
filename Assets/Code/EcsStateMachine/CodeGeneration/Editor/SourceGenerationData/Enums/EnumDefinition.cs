using System.Collections.Generic;

namespace Code.EcsStateMachine.CodeGeneration.Editor.SourceGenerationData.Enums
{
    public sealed class EnumDefinition
    {
        public string Namespace = string.Empty;
        public string Name = string.Empty;
        public IReadOnlyList<EnumValue> Values = new List<EnumValue>();
    }
}

