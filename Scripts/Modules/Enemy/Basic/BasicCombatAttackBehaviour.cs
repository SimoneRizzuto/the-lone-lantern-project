using System;
using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicCombatAttackBehaviour : BaseEnemyBehaviour
{
    private int attackSpeed = 10;
    private bool applyingVelocity;
    private Vector2 DirectionToPlayer => StateMachine.EnemyTemplate.Position.DirectionTo(Luce.Position);
    
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.CombatAttack)
        {
            applyingVelocity = false;
            return;
        }

        if (!Timer.IsRunning)
        {
            Timer.Restart();
        }
        
        if (!applyingVelocity)
        {
            StateMachine.EnemyTemplate.CalculatedVelocity = DirectionToPlayer * EnemyConstants.AttackSpeed;
            applyingVelocity = true;
        }

        if (Timer.Elapsed > TimeSpan.FromSeconds(0.25))
        {
            Timer.Reset();
            StateMachine.EnemyState = EnemyState.CombatWait;
        }
    }
    
    // attack objects can probably be static in here.
    // each attack needs to be assignable for this Behavior to find it.
    
    // Because each Behaviour is custom, instancing the attacks statically is probably fine?
    // experiment with two attacks in here
    // consider: animations, hitboxes, length of attack, what state the enemy will go into
    // all of this makes me think each shouldn't be a static object, but instead a PackedScene???
    // and each packed scene gets added as a child or some other way??????
}