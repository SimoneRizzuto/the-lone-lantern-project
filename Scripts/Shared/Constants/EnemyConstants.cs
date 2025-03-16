namespace TheLoneLanternProject.Scripts.Shared.Constants;

public enum EnemyState
{
    OutOfCombatIdle,
    OutOfCombatMove,
    CombatWait,
    CombatReposition,
    CombatAttack,
    CombatHurt,
    CombatKnockBack,
    CombatDodge,
    CombatParry,
}

public static class EnemyConstants
{
    public const float MoveSpeed = 4000;
    public const float AttackSpeed = 10000;
    
    public const float InitiateCombatDistance = 125;
    public const float AttackDistance = 50;
}