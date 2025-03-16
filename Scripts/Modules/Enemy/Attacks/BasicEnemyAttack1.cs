using System;
using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Attacks;

[GlobalClass]
public partial class BasicEnemyAttack1 : BaseEnemyAttack, IDisposable
{
    private string LastDirectionString => Enum.GetName(StateMachine.LastDirection)?.ToLower();
    
    private bool applyingVelocity;
    private Vector2 aimedPositionToAttack;
    
    public override void _Ready()
    {
        if (string.IsNullOrWhiteSpace(AttackName)) AttackName = "Test";
        if (AttackSpeed < 0) AttackSpeed = EnemyConstants.AttackSpeed;
        if (AttackLength < 0) AttackLength = 0.25f;
        if (string.IsNullOrWhiteSpace(AttackAnimationName)) AttackAnimationName = "Enemy Name";
        if (AttackAnimationHitBoxBeginFrame < 0) AttackAnimationHitBoxBeginFrame = 0;
        if (AttackAnimationHitBoxFinishFrame < 0) AttackAnimationHitBoxFinishFrame = 1;
        
        MainSprite.AnimationLooped += StopWindingUp;
        MainSprite.AnimationLooped += StopAttacking;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.CombatAttack || !Triggered)
        {
            Triggered = false;
            applyingVelocity = false;
            Hitbox.Monitoring = false;
            return;
        }
        
        if (!Timer.IsRunning)
        {
            Timer.Restart();
            WindingUp = true;
        }
        
        if (!applyingVelocity || !WindingUp)
        {
            ProcessAttack();
        }
    }
    
    private void ProcessAttack()
    {
        if (WindingUp)
        {
            var attackingDirection = DirectionHelper.GetSnappedDirection(DirectionToPlayer);
            if (attackingDirection == Direction.Up || attackingDirection == Direction.Down)
            {
                switch (DirectionToPlayer.X)
                {
                    case < 0:
                        attackingDirection = Direction.Left;
                        break;
                    case >= 0:
                        attackingDirection = Direction.Right;
                        break;
                }
                
                StateMachine.LastDirection = attackingDirection;
            }
            
            var animationToPlay = $"windup {LastDirectionString} 1";
            MainSprite.Play(animationToPlay);
            
            aimedPositionToAttack = DirectionToPlayer;
            
            AdjustHitboxPosition();
        }
        else if (!WindingUp)
        {
            var animationToPlay = $"attack {LastDirectionString} 1";
            MainSprite.Play(animationToPlay);
            
            StateMachine.EnemyTemplate.CalculatedVelocity = aimedPositionToAttack * AttackSpeed;
            Hitbox.Monitoring = true;
            applyingVelocity = true;
        }
    }
    
    private void AdjustHitboxPosition()
    {
        if (StateMachine.LastDirection == Direction.Left)
        {
            var x = Math.Abs(HitboxCollisionShape.Position.X) * -1;
            var y = HitboxCollisionShape.Position.Y;
            
            HitboxCollisionShape.Position = new Vector2(x, y);
        }
        else if (StateMachine.LastDirection == Direction.Right)
        {
            var x = Math.Abs(HitboxCollisionShape.Position.X);
            var y = HitboxCollisionShape.Position.Y;
            
            HitboxCollisionShape.Position = new Vector2(x, y);
        }
    }
    
    // signals
    private void StopWindingUp()
    {
        if (MainSprite.Animation.ToString().Contains("windup"))
        {
            WindingUp = false;
        }
    }
    private void StopAttacking()
    {
        if (MainSprite.Animation.ToString().Contains("attack"))
        {
            var animationToPlay = $"idle {LastDirectionString}";
            MainSprite.Play(animationToPlay);
            
            Timer.Reset();
            StateMachine.EnemyState = EnemyState.CombatWait;
        }
    }

    // disposable subscribed methods
    public void Dispose()
    {
        MainSprite.AnimationLooped -= StopWindingUp;
        MainSprite.AnimationLooped -= StopAttacking;
    }
}