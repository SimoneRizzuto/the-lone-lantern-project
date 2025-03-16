using System;
using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Attacks;

[GlobalClass]
public partial class BasicEnemyAttack1 : BaseEnemyAttack
{
    private bool applyingVelocity;
    
    public override void _Ready()
    {
        if (string.IsNullOrWhiteSpace(AttackName)) AttackName = "Test";
        if (AttackSpeed < 0) AttackSpeed = EnemyConstants.AttackSpeed;
        if (AttackLength < 0) AttackLength = 0.25f;
        if (string.IsNullOrWhiteSpace(AnimationName)) AnimationName = "Enemy Name";
        if (AttackAnimationHitBoxBeginFrame < 0) AttackAnimationHitBoxBeginFrame = 0;
        if (AttackAnimationHitBoxFinishFrame < 0) AttackAnimationHitBoxFinishFrame = 1;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.CombatAttack || !Triggered)
        {
            Triggered = false;
            applyingVelocity = false;
            return;
        }
        
        if (!Timer.IsRunning)
        {
            Timer.Restart();
        }
        
        if (!applyingVelocity)
        {
            StateMachine.EnemyTemplate.CalculatedVelocity = DirectionToPlayer * AttackSpeed;
            
            var direction = SetAttackingDirection(DirectionToPlayer);
            SetAttackingAnimation(direction, DirectionToPlayer);
            
            applyingVelocity = true;
        }
        
        if (Timer.Elapsed > TimeSpan.FromSeconds(0.25))
        {
            Timer.Reset();
            StateMachine.EnemyState = EnemyState.CombatWait;
        }
    }
    
    private Direction SetAttackingDirection(Vector2 directionVector)
    {
        var walkingDirection = DirectionHelper.GetSnappedDirection(directionVector);
        StateMachine.LastDirection = walkingDirection;
        return walkingDirection;
    }
    
    private void SetAttackingAnimation(Direction attackingDirection, Vector2 directionVector)
    {
        var lastDirectionString = Enum.GetName(attackingDirection)?.ToLower();
        var animationToPlay = $"attack {lastDirectionString} 1";
        var speed = Mathf.Snapped(directionVector.Length(), 2);
        MainSprite.Play(animationToPlay, speed);
    }
}