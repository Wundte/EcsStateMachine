namespace Code.Data.Ecs
{
    public enum SystemType
    {
        Init = 0,
        Update = 1,
        StateChange = 4,
        OneShot = 5,
    }
}