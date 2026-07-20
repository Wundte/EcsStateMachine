using System.IO;

namespace Code.EcsStateMachine.Editor.CodeGeneration.Output
{
    public static class FileWriter
    {
        private const string GeneratedFolder = "Assets/Code/EcsStateMachine/Runtime/Generated";
    
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

