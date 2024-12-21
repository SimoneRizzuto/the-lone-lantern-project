using System;
using Godot;
using TheLoneLanternProject.Scripts.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy;

[GlobalClass]
public partial class EnemyWaitingModule : Node
{
    [Export] public StateMachines.Enemy.EnemyStateMachine State;

    //lambda expression which evaluates if state was waiting
    private bool StateIsValid => State.EnemyState != EnemyState.Waiting;
    
    public override void _Ready()
    {
        // Get state of the parent
        State ??= GetParent<StateMachines.Enemy.EnemyStateMachine>();
    }

    public override void _Process(double delta)
    {
        if (StateIsValid)
        {
            return;
        }
        // set the animation to waiting in the last direction if the state is waiting
        State.MainSprite.Animation = $"waiting {Enum.GetName(State.LastDirection)?.ToLower()}";
    }
}
