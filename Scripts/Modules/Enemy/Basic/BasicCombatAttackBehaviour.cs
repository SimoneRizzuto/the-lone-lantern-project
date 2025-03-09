using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicCombatAttackBehaviour : BaseEnemyBehaviour
{
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.CombatAttack) return;
    }
}