using System.Text;
using Code.EcsStateMachine.CodeGeneration.Editor.SourceGenerationData.Enums;

namespace Code.EcsStateMachine.CodeGeneration.Editor.Generation.Types
{
    public sealed class EnumGenerator
    {
        public static string GenerateEnum(EnumDefinition definition)
        {
            var builder = new StringBuilder();
        
            builder.AppendLine("// Generated enum");
            builder.AppendLine();
            builder.AppendLine($"namespace {definition.Namespace}");
            builder.AppendLine("{");
            builder.AppendLine($"    public enum {definition.Name}");
            builder.AppendLine("    {");
            builder.AppendLine($"        None,");
            foreach (var value in definition.Values)
            {
                builder.AppendLine($"        {value.Name} = {value.Value},");
            }
            builder.AppendLine("    }");
            builder.AppendLine("}");
        
            return builder.ToString();
        }
    }
}

