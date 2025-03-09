using Godot;
using System;
using Vector2 = Godot.Vector2;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Extensions;
using TheLoneLanternProject.Scripts.Shared.Helpers;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicOutOfCombatMoveBehaviour : BaseEnemyBehaviour
{
    private Vector2 direction;
    private int secondsToMove;
    
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.OutOfCombatMove) return;
        
        if (!Timer.IsRunning)
        {
            var random = new Random();
            
            direction = VectorExtensions.GetRandomDirection(-1, 1);
            secondsToMove = random.Next(1, 3);
            Timer.Restart();
        }
        
        StateMachine.EnemyTemplate.CalculatedVelocity = direction * 2000f;
        
        var walkingDirection = DirectionHelper.GetSnappedDirection(direction);
        StateMachine.LastDirection = walkingDirection;
        
        var lastDirectionString = Enum.GetName(walkingDirection)?.ToLower();
        var animationToPlay = $"walk {lastDirectionString}";
        MainSprite.Play(animationToPlay);
        
        if (Timer.Elapsed > TimeSpan.FromSeconds(secondsToMove))
        {
            StateMachine.EnemyState = EnemyState.OutOfCombatIdle;
            StateMachine.EnemyTemplate.CalculatedVelocity = Vector2.Zero;
            Timer.Reset();
        }
    }
}