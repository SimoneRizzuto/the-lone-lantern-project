using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Modules;
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

    private bool MovementVectorIsAboveThreshold(Vector2 vector) => vector.Length() > MovementVectorThreshold;

    public override void _PhysicsProcess(double delta)
    {
        if (StateIsValid) return;

        //var movementVector = // here it would have been the direction of the input from the player but it now needs to be autonomous
        // I need reference to luce and I need to have some logic that moves in his direction (for now and might need to be
        // extended if all enemies are meant to have some hive mind).
        //State.Enemy.CalculatedVelocity = MovementVectorIsAboveThreshold(movementVector) ? movementVector * MoveSpeed : Vector2.Zero;
        // This also needs to be considered. Previously the framework had a parent (luce3) that had its own script that did this
        // stuff and set up CalculatedVelocity. Need to make one of these or find another solution
        // Check with Sim about moving some stuff into luce3
        SetMovementAnimation(movementVector);
    }

    private void SetMovementAnimation(Vector2 movementVector)
    {
        var walkDirection = DirectionHelper.GetSnappedDirection(movementVector, MovementVectorThreshold);

        var isWalking = walkDirection != Direction.None;
        if (isWalking)
        {
            // Will need to make it so that future enemies that get added follow this structure for naming.
            var animation = $"walk {Enum.GetName(walkDirection)?.ToLower()}";
            var speed = Mathf.Snapped(movementVector.Length(), MovementVectorThreshold * 2);
            State.MainSprite.Play(animation, speed);

            State.LastDirection = walkDirection;
            State.EnemyState = EnemyState.Reposition;
        }
        else
        {
            State.EnemyState = EnemyState.Waiting;
        }

        State.MainSprite.Play();
    }
}
