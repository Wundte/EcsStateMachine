namespace Code.EcsStateMachine.Editor.CodeGeneration.Utils
{
    /// <summary>
    /// Generates stable identifiers from strings.
    /// </summary>
    internal static class StableId
    {
        /// <summary>
        /// Calculates deterministic hash value.
        /// </summary>
        public static int Get(string text)
        {
            unchecked
            {
                var hash = 17;

                foreach (var c in text)
                {
                    hash = hash * 31 + c;
                }

                return hash;
            }
        }
    }
}