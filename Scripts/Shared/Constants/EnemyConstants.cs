namespace TheLoneLanternProject.Scripts.Shared.Constants;

public enum EnemyState
{
    OutOfCombatIdle,
    OutOfCombatMove,
    CombatWait,
    CombatReposition,
    CombatAttack,
    CombatHurt,
}

public static class EnemyConstants
{
    public const float CombatDistanceThreshold = 125;
}