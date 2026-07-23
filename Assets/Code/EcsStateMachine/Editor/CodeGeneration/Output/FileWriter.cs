using System.IO;

namespace Code.EcsStateMachine.Editor.CodeGeneration.Output
{
    /// <summary>
    /// Writes generated source files to the project.
    /// </summary>
    internal static class FileWriter
    {
        private const string GeneratedFolder = "Assets/Code/EcsStateMachine/Runtime/Generated";
    
        /// <summary>
        /// Writes content to generated file.
        /// </summary>
        public static void Write(string fileName, string content)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                GeneratedFolder,
                fileName);
        
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, content);
        }
    }
}