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
}

public static class NodeGroup
{
    public const string Player = "player";
    public const string Attack = "attack";
    public const string Door = "door";
    public const string Enemy = "enemy";
    public const string ActorNode = "actor node";
    public const string CutsceneDirector = "cutscene director";
    public const string PlayerCamera = "player camera";
    public const string MainCamera = "main camera";
}