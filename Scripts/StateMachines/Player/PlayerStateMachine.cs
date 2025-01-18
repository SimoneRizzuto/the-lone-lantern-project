using Godot;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Modules.DustCloud;
using TheLoneLanternProject.Scripts.Modules.StaminaHealth;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.StateMachines.Player;

[GlobalClass]
public partial class PlayerStateMachine : Base.StateMachine
{
    [Export] public Luce Player;
    [Export] public PlayerState PlayerState = PlayerState.Idle;
    [Export] public Direction LastDirection = Direction.Down;
    [Export] public AnimatedSprite2D MainSprite;
    [Export] public DustCloudModule DustCloudModule;
    [Export] public StaminaHealthModule StaminaHealthModule;

    public bool IsInteracting;
    
    public override void _EnterTree()
    {
        var owner = Owner;
        
        Player ??= (Luce)owner;
        MainSprite ??= owner.GetNode<AnimatedSprite2D>("MainSprite");
        DustCloudModule ??= owner.GetNode<DustCloudModule>("DustCloudModule");
        StaminaHealthModule ??= owner.GetNode<StaminaHealthModule>("StaminaHealthModule");
    }
}