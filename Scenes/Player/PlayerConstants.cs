using System;

namespace TheLoneLanternProject.Scenes.Player;

[Flags]
public enum PlayerState
{
    Idle = 0,               // 0
    Walking = 1 << 0,       // 1
    Attacking = 1 << 1,     // 2
    Dashing = 1 << 2,       // 4
    Hurting = 1 << 3,       // 8
    Disabled = 1 << 4,      // 16
}

public enum StaminaHealthState
{
    Max = 0,
    Pause = 1,
    Regen = 2,
}