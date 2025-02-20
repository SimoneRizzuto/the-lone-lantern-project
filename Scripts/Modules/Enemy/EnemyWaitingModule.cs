using Godot;
using System;
using TheLoneLanternProject.Scripts.Shared.Constants;
//using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy;

[GlobalClass]
public partial class EnemyWaitingModule : Node
{
    [Export] public EnemyStateMachine State;

    //lambda expression which evaluates if state was waiting
    private bool StateIsValid => State.EnemyState != EnemyState.Waiting;
    
    public override void _Ready()
    {
        // Get state of the parent
        State ??= GetParent<EnemyStateMachine>();
    }

    public override void _Process(double delta)
    {
        GD.Print(State.EnemyState);
        if (StateIsValid)
        {
            return;
        }
        // set the animation to waiting in the last direction if the state is waiting
        State.MainSprite.Animation = $"waiting {Enum.GetName(State.LastDirection)?.ToLower()}";
    }
}
