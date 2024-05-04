namespace TheLoneLanternProject.Scenes.Player;

public enum PlayerState
{
    Idle,
    Walking,
    Attacking,
    Hurting,
    Disabled
}

public enum StaminaHealthState
{
    Max = 0,
    Pause = 1,
    Regen = 2
}