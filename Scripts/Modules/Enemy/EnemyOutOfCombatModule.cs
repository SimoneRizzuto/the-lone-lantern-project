using System;
using Godot;
using TheLoneLanternProject.Modules;
using TheLoneLanternProject.Scripts.Constants;
using TheLoneLanternProject.Scripts.Helpers;

namespace TheLoneLanternProject.Scripts.Modules.Enemy;

[GlobalClass]
public partial class EnemyOutOfCombatModule : Node
{
    [Export] public StateMachines.Enemy.EnemyStateMachine State;

    private Scripts.Player.Luce luce;
    private bool StateIsOutOfCombat => State.EnemyState is EnemyState.OutOfCombat;
    private static readonly float combatDistanceThreshold = 50;

    public override void _Ready()
    {
        State ??= GetParent<StateMachines.Enemy.EnemyStateMachine>();

    }

    public override void _PhysicsProcess(double delta)
    {
        CheckDistanceToLuce();

        if (!StateIsOutOfCombat) return;

        // Just make out of combat animation the same as waiting for now
        State.MainSprite.Animation = $"waiting {Enum.GetName(State.LastDirection)?.ToLower()}";


    }

    private void CheckDistanceToLuce()
    {
        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);

        var distance = State.Enemy.Position.DistanceTo(luce.Position); // just check that this works
        if (distance <= combatDistanceThreshold)
        {
            State.EnemyState = EnemyState.Reposition;
        }
        else
        {
            State.EnemyState = EnemyState.OutOfCombat;
        }
    }
}
