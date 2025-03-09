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
        //CheckDistanceToLuce();
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
        MainSprite.Play($"idle {lastDirectionString}");
        
        // implement new logic
        // do movement here? or somewhere else?
        // probably here. there doesn't always need to be one place where everything's done, but think about it some more before going with it

        // changed my mind, i want to make every bit of state something different. OutOfCombat isn't one state, it is OutOfCombatIdle, OutOfCombatMove

        // worry about animation after
    }
    
    private void CheckDistanceToLuce()
    {
        var distance = StateMachine.EnemyTemplate.Position.DistanceTo(Luce.Position); // just check that this works
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