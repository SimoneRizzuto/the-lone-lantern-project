using Godot;
using System;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicCombatRepositionBehaviour : BaseEnemyBehaviour
{
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.CombatReposition) return;
        
        var movementVector = StateMachine.EnemyTemplate.Position.DirectionTo(Luce.Position); 
        StateMachine.EnemyTemplate.CalculatedVelocity = movementVector * EnemyConstants.MoveSpeed;
        SetMovementAnimation(movementVector);
        
        // This also needs to be considered. Previously the framework had a parent (luce3) that had its own script that did this
        // stuff and set up CalculatedVelocity. Need to make one of these or find another solution
        // Check with Sim about moving some stuff into luce3
    }
    
    private void SetMovementAnimation(Vector2 movementVector)
    {
        var walkDirection = DirectionHelper.GetSnappedDirection(movementVector);
        
        var isWalking = walkDirection != Direction.None;
        if (isWalking)
        {
            // Will need to make it so that future enemies that get added follow this structure for naming.
            var animation = $"walk {Enum.GetName(walkDirection)?.ToLower()}";
            var speed = Mathf.Snapped(movementVector.Length(), 2);
            MainSprite.Play(animation, speed);

            StateMachine.LastDirection = walkDirection;
            StateMachine.EnemyState = EnemyState.CombatAttack;
        }
        else
        {
            StateMachine.EnemyState = EnemyState.CombatWait;
        }

        MainSprite.Play();
    }
}