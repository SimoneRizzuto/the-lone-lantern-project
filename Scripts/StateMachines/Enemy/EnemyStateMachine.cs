using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.StateMachines.Enemy;

[GlobalClass]
public partial class EnemyStateMachine : Base.StateMachine
{
    [Export] public Enemies.Enemy Enemy; // Placeholder in case the enemy needs to be supplied like luce is supplied
    // Initialise State and Direction
    [Export] public EnemyState EnemyState = EnemyState.Waiting;
    [Export] public Direction LastDirection = Direction.Down;

    // Make it so the enemy sprite can be passed
    [Export] public AnimatedSprite2D MainSprite;

    public override void _Ready()
    {
        var owner = Owner;

        Enemy ??= (Enemies.Enemy)owner;
        MainSprite ??= owner.GetNode<AnimatedSprite2D>("MainSprite");
    }

}
