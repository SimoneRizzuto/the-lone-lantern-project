namespace TheLoneLanternProject.Constants;

public enum Direction
{
    Up = 0,
    Left = 1,
    Right = 2,
    Down = 3
}

public static class InputMapAction
{
    public const string Up = "Move Up";
    public const string Down = "Move Down";
    public const string Left = "Move Left";
    public const string Right = "Move Right";
    public const string Attack = "Attack";
    public const string Enter = "Enter";
    public const string Save = "Save";
    public const string Load = "Load";
}

public static class NodeGroup
{
    public const string Player = "player";
    public const string Attack = "attack";
    public const string Door = "door";
    public const string Enemy = "enemy";
}