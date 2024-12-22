using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.StateMachines.Player;

namespace TheLoneLanternProject.Scripts.Modules.Player;

[GlobalClass]
public partial class PlayerHurtingModule : Node
{
    [Export] public PlayerStateMachine State;
    
    public override void _Ready()
    {
        State ??= GetParent<PlayerStateMachine>();
    }

    public override void _Process(double delta)
    {
        if (State.PlayerState != PlayerState.Hurting) return;
        
        
    }
}