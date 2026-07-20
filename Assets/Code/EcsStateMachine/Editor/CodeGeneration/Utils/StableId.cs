namespace Code.EcsStateMachine.Editor.CodeGeneration.Utils
{
    public static class StableId
    {
        /// <summary>
        /// Get hash from system name.
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

