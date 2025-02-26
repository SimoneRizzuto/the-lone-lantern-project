using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.StateMachines.Base;


[GlobalClass]
public partial class EnemyStateMachine : StateMachine
{
    [Export] public Enemy Enemy; 
    [Export] public EnemyState EnemyState = EnemyState.OutOfCombat;
    [Export] public Direction LastDirection = Direction.Down;

    // Make it so the enemy sprite can be passed
    [Export] public AnimatedSprite2D MainSprite;

    public override void _EnterTree()
    {
        var owner = Owner;

        Enemy ??= (Enemy)owner;
        MainSprite ??= owner.GetNode<AnimatedSprite2D>("MainSprite");
        
    }

}
