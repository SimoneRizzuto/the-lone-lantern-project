using Godot;
using System;
using System.Diagnostics;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicOutOfCombatIdleBehaviour : BaseEnemyBehaviour
{
    private int secondsToMove;
    
    public override void _PhysicsProcess(double delta)
    {
        CheckDistanceToLuce();
        
        if (StateMachine.EnemyState is not EnemyState.OutOfCombatIdle) return;
        
        if (!Timer.IsRunning)
        {
            var random = new Random();
            
            secondsToMove = random.Next(1, 3);
            Timer.Restart();
        }
        
        if (Timer.Elapsed > TimeSpan.FromSeconds(secondsToMove))
        {
            StateMachine.EnemyState = EnemyState.OutOfCombatMove;
            Timer.Reset();
        }
        
        var lastDirectionString = Enum.GetName(StateMachine.LastDirection)?.ToLower();
        var animationToPlay = $"idle {lastDirectionString}";
        MainSprite.Play(animationToPlay);
    }
    
    private void CheckDistanceToLuce()
    {
        var distance = StateMachine.EnemyTemplate.Position.DistanceTo(Luce.Position);
        if (distance <= EnemyConstants.CombatDistanceThreshold)
        {
            StateMachine.EnemyState = EnemyState.Reposition;
        }
        else
        {
            StateMachine.EnemyState = EnemyState.OutOfCombatIdle;
        }
    }
}