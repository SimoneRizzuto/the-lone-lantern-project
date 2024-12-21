using System;
using Godot;
using TheLoneLanternProject.Modules;
using TheLoneLanternProject.Scripts.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Player;

[GlobalClass]
public partial class PlayerIdleModule : Node
{
    [Export] public StateMachines.Player.PlayerStateMachine State;
    
    private bool StateIsValid => State.PlayerState != PlayerState.Idle;
    
    public override void _Ready()
    {
        State ??= GetParent<StateMachines.Player.PlayerStateMachine>();
    }
    
    public override void _Process(double delta)
    {
        if (StateIsValid) return;
        State.MainSprite.Animation = $"idle {Enum.GetName(State.LastDirection)?.ToLower()}";
    }
}