namespace Code.EcsStateMachine.Runtime.Data.Constants
{
    /// <summary>
    /// Defines ECS system execution groups.
    /// </summary>
    internal enum EcsSystemType
    {
        /// <summary>
        /// Systems executed during state changes.
        /// </summary>
        StateChangeSystems = 10,

        /// <summary>
        /// Regular update systems.
        /// </summary>
        Run = 20, 
    }
}