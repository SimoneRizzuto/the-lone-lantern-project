using System;
using Godot;
using System.Diagnostics;
using System.Numerics;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Extensions;
using Vector2 = Godot.Vector2;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicOutOfCombatMoveBehaviour : BaseEnemyBehaviour
{
    private Stopwatch stopwatch = new();
    
    private Vector2 direction;
    private int secondsToMove;
    
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.OutOfCombatMove) return;
        
        if (!stopwatch.IsRunning)
        {
            var random = new Random();
            
            direction = VectorExtensions.GetRandomDirection(-1, 1);
            secondsToMove = random.Next(1, 3);
            stopwatch.Restart();
        }
        
        StateMachine.EnemyTemplate.CalculatedVelocity = direction * 2000f;
        
        if (stopwatch.Elapsed > TimeSpan.FromSeconds(secondsToMove))
        {
            StateMachine.EnemyState = EnemyState.OutOfCombatIdle;
            StateMachine.EnemyTemplate.CalculatedVelocity = Vector2.Zero;
            stopwatch.Reset();
        }
    }
}