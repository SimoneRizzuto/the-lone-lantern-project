using Godot;
using System;
using System.Diagnostics;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicOutOfCombatIdleBehaviour : BaseBasicEnemyBehaviour
{
    private Stopwatch stopwatch = new();
    
    public override void _PhysicsProcess(double delta)
    {
        //CheckDistanceToLuce();
        if (StateMachine.EnemyState is not EnemyState.OutOfCombatIdle) return;
        
        if (!stopwatch.IsRunning)
        {
            stopwatch.Restart();
        }
        
        var seconds = 3; // make random number
        if (stopwatch.Elapsed == TimeSpan.FromSeconds(seconds))
        {
            StateMachine.EnemyState = EnemyState.OutOfCombatMove;
        }

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