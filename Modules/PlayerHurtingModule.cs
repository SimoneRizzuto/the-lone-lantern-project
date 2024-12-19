using Godot;
using TheLoneLanternProject.Scripts.Constants;

namespace TheLoneLanternProject.Modules;

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