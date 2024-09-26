using System;
using Godot;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Modules;

[GlobalClass]
public partial class PlayerDisabledModule : Node
{
    [Export] public PlayerStateMachine State;

    public override void _Ready()
    {
        State ??= GetParent<PlayerStateMachine>();
    }

    public override void _Process(double delta)
    {
        if (State.PlayerState != PlayerState.Disabled) return;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
    }
}