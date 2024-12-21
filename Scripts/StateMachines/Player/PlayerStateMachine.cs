using Godot;
using TheLoneLanternProject.Scripts.Constants;
using TheLoneLanternProject.Scripts.Modules.DustCloud;
using TheLoneLanternProject.Scripts.Player;

namespace TheLoneLanternProject.Scripts.StateMachines.Player;

[GlobalClass]
public partial class PlayerStateMachine : Base.StateMachine
{
    [Export] public Luce Player;
    [Export] public PlayerState PlayerState = PlayerState.Idle;
    [Export] public Direction LastDirection = Direction.Down;
    [Export] public AnimatedSprite2D MainSprite;
    [Export] public DustCloudModule DustCloudModule;
    [Export] public Modules.StaminaHealth.StaminaHealthModule StaminaHealthModule;
    
    public override void _EnterTree()
    {
        var owner = Owner;
        
        Player ??= (Luce)owner;
        MainSprite ??= owner.GetNode<AnimatedSprite2D>("MainSprite");
        DustCloudModule ??= owner.GetNode<DustCloudModule>("DustCloudModule");
        StaminaHealthModule ??= owner.GetNode<Modules.StaminaHealth.StaminaHealthModule>("StaminaHealthModule");
    }
}