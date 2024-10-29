namespace TheLoneLanternProject.Constants;

public enum Direction
{
    Up = 0,
    Left = 1,
    Right = 2,
    Down = 3,
    None = 4,
}

public static class InputMapAction
{
    public const string Up = "Move Up";
    public const string Down = "Move Down";
    public const string Left = "Move Left";
    public const string Right = "Move Right";
    public const string Attack = "Attack";
    public const string Dash = "Dash";
    public const string Enter = "Enter";
    public const string Save = "Save";
    public const string Load = "Load";
    public const string Interact = "Interact";
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
    public const string Weather = "weather";
    public const string TransitionCamera = "transition camera";
    public const string Persist = "Persist";
    public const string AudioDirector = "audio director";
}

public static class PlayerConstants
{
    public const int MoveSpeed = 5000;
}
