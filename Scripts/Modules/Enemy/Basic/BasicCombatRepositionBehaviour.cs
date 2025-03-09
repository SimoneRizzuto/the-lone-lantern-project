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
        
        var directionVector = SetDirectionVector();
        var walkingDirection = SetWalkingDirection(directionVector);
        
        SetWalkingAnimation(walkingDirection, directionVector);
        
        var distance = StateMachine.EnemyTemplate.Position.DistanceTo(Luce.Position);
        if (distance <= EnemyConstants.AttackDistance)
        {
            StateMachine.EnemyState = EnemyState.CombatAttack;
        }
    }
    
    private Vector2 SetDirectionVector()
    {
        var direction = StateMachine.EnemyTemplate.Position.DirectionTo(Luce.Position);
        StateMachine.EnemyTemplate.CalculatedVelocity = direction * EnemyConstants.MoveSpeed;
        return direction;
    }
    
    private Direction SetWalkingDirection(Vector2 directionVector)
    {
        var walkingDirection = DirectionHelper.GetSnappedDirection(directionVector);
        StateMachine.LastDirection = walkingDirection;
        return walkingDirection;
    }
    
    private void SetWalkingAnimation(Direction walkingDirection, Vector2 directionVector)
    {
        var lastDirectionString = Enum.GetName(walkingDirection)?.ToLower();
        var animationToPlay = $"walk {lastDirectionString}";
        var speed = Mathf.Snapped(directionVector.Length(), 2);
        MainSprite.Play(animationToPlay, speed);
    }
}