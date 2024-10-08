using System;
using Godot;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Modules;

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