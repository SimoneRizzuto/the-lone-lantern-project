namespace TheLoneLanternProject.Scenes.Player;

public enum Direction
{
    Left,
    Right
}
public enum PlayerState
{
    Idle,
    Walk,
    Attack,
    Hurt,
    Disabled
}

public enum StaminaHealthState
{
    Max = 0,
    Pause = 1,
    Regen = 2
}