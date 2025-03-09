using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Enemies.Templates;
using TheLoneLanternProject.Scripts.StateMachines.Base;

namespace TheLoneLanternProject.Scripts.StateMachines.Enemy;

[GlobalClass]
public partial class EnemyStateMachine : StateMachine
{
    public EnemyTemplate EnemyTemplate;
    public EnemyState EnemyState = EnemyState.OutOfCombatIdle;
    public Direction LastDirection = Direction.Down;

    public override void _Ready()
    {
        EnemyTemplate ??= (EnemyTemplate)Owner;
    }
}
