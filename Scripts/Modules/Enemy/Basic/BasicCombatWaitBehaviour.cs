using System;
using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicCombatWaitBehaviour : BaseEnemyBehaviour
{
    private int secondsToWait = 1;
    
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.CombatWait) return;
        
        CheckDistanceToLuce();
        
        StateMachine.EnemyTemplate.CalculatedVelocity = Vector2.Zero;
        
        if (!Timer.IsRunning)
        {
            Timer.Restart();
            
            var random = new Random();
            secondsToWait = random.Next(1, 4);
        }
        
        var lastDirectionString = Enum.GetName(StateMachine.LastDirection)?.ToLower();
        var animationToPlay = $"idle {lastDirectionString}";
        MainSprite.Play(animationToPlay);
        
        if (Timer.Elapsed > TimeSpan.FromSeconds(secondsToWait))
        {
            StateMachine.EnemyState = EnemyState.CombatAttack;
            Timer.Reset();
        }
    }
}