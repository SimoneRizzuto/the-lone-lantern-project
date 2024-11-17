using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Modules;

[GlobalClass]
public partial class EnemyStateMachine : StateMachine
{
    // [Export] public EnemyClass Enemy; // Placeholder in case the enemy needs to be supplied like luce is supplied
    // Initialise State and Direction
    [Export] public EnemyState EnemyState = EnemyState.Waiting;
    [Export] public Direction LastDirection = Direction.Down;

    // Make it so the enemy sprite can be passed
    [Export] public AnimatedSprite2D MainSprite;

    public override void _Ready()
    {
        var owner = Owner;

        //Enemy??= (EnemyClass)owner;
        MainSprite ??= owner.GetNode<AnimatedSprite2D>("MainSprite");
    }

}
