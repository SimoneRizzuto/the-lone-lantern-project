using Godot;
using TheLoneLanternProject.Modules;
using TheLoneLanternProject.Scripts.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Player;

[GlobalClass]
public partial class PlayerDisabledModule : Node
{
    [Export] public StateMachines.Player.PlayerStateMachine State;

    public override void _Ready()
    {
        State ??= GetParent<StateMachines.Player.PlayerStateMachine>();
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