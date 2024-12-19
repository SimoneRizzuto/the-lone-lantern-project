using Godot;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;
using TheLoneLanternProject.Modules.DustCloud;

namespace TheLoneLanternProject.Modules;

[GlobalClass]
public partial class PlayerStateMachine : StateMachine
{
    [Export] public Luce Player;
    [Export] public PlayerState PlayerState = PlayerState.Idle;
    [Export] public Direction LastDirection = Direction.Down;
    [Export] public AnimatedSprite2D MainSprite;
    [Export] public DustCloudModule DustCloudModule;
    [Export] public StaminaHealthModule StaminaHealthModule;
    
    public override void _EnterTree()
    {
        var owner = Owner;
        
        Player ??= (Luce)owner;
        MainSprite ??= owner.GetNode<AnimatedSprite2D>("MainSprite");
        DustCloudModule ??= owner.GetNode<DustCloudModule>("DustCloudModule");
        StaminaHealthModule ??= owner.GetNode<StaminaHealthModule>("StaminaHealthModule");
    }
}