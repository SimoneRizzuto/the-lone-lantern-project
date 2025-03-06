using System;

namespace TheLoneLanternProject.Scripts.Shared.Constants;

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

public enum EnemyState
{
    OutOfCombat = 0,
    Waiting = 1,
    Reposition = 2,
    Attacking = 3,
    Hurting = 4,
}

public enum StaminaHealthState
{
    Max = 0,
    Pause = 1,
    Regen = 2,
}