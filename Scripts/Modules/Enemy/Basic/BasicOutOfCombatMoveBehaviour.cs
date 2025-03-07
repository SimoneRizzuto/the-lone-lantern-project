using System;
using Godot;
using System.Diagnostics;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicOutOfCombatMoveBehaviour : BaseEnemyBehaviour
{
    private Stopwatch stopwatch = new();
    
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState != EnemyState.OutOfCombatIdle) return;
        
        if (!stopwatch.IsRunning)
        {
            stopwatch.Restart();
        }
        
        var seconds = 3; // make random number
        if (stopwatch.Elapsed == TimeSpan.FromSeconds(seconds))
        {
            StateMachine.EnemyState = EnemyState.OutOfCombatIdle;
        }
    }
}