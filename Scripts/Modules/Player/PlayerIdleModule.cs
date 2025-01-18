using Godot;
using System;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.StateMachines.Player;

namespace TheLoneLanternProject.Scripts.Modules.Player;

[GlobalClass]
public partial class PlayerIdleModule : Node
{
    [Export] public PlayerStateMachine State;
    
    private bool StateIsValid => State.PlayerState != PlayerState.Idle;
    
    public override void _Ready()
    {
        State ??= GetParent<PlayerStateMachine>();
    }
    
    public override void _Process(double delta)
    {
        if (StateIsValid) return;
        
        State.MainSprite.Animation = $"idle {Enum.GetName(State.LastDirection)?.ToLower()}";
    }
}