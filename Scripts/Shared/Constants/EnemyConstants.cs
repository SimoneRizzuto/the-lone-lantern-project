namespace TheLoneLanternProject.Scripts.Shared.Constants;

public enum EnemyState
{
    OutOfCombatIdle,
    OutOfCombatMove,
    Waiting,
    Reposition,
    Attacking,
    Hurting,
}

public static class EnemyConstants
{
    public const float CombatDistanceThreshold = 125;
}