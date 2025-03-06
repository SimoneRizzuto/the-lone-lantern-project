using System;
using System.Numerics;
using Godot;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Extensions;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.StateMachines.Enemy;
using Vector2 = Godot.Vector2;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicOutOfCombatBehaviour : Node
{
    private EnemyStateMachine State => GetParent<EnemyStateMachine>();
    private Luce Luce => GetNodeHelper.GetLuce(GetTree());
    
    public override void _PhysicsProcess(double delta)
    {
        //CheckDistanceToLuce();
        if (State.EnemyState is not EnemyState.OutOfCombat) return;
        
        var direction = VectorExtensions.GetRandomDirection(-1, 1);;
        State.EnemyTemplate.CalculatedVelocity = 5000 * direction;
        
        //Console.WriteLine("Direction:" + direction);

        // implement new logic
        // do movement here? or somewhere else?
        // probably here. there doesn't always need to be one place where everything's done, but think about it some more before going with it


        // worry about animation after
    }
    
    

    private void CheckDistanceToLuce()
    {
        var distance = State.EnemyTemplate.Position.DistanceTo(Luce.Position); // just check that this works
        if (distance <= EnemyConstants.CombatDistanceThreshold)
        {
            State.EnemyState = EnemyState.Reposition;
        }
        else
        {
            State.EnemyState = EnemyState.OutOfCombat;
        }
    }
}