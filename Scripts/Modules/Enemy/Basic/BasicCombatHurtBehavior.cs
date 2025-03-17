using Godot;
using TheLoneLanternProject.Scripts.Modules.Bars.Health;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicCombatHurtBehavior : BaseEnemyBehaviour
{
    private HealthModule Health => GetParent().GetParent().GetNode<HealthModule>("HealthModule");
    
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.CombatHurt) return;
        
        // apply damage
        
        Health.DealDamage();
        
        // apply knockback vector
        // how long with the knockback?
        
        StateMachine.EnemyState = EnemyState.CombatKnockBack;
        
        // reset value
    }
}