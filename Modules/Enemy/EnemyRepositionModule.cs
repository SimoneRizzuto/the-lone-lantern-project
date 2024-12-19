using Godot;
using System;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.DirectionHelpers;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Modules;

[GlobalClass]
public partial class EnemyRepositionModule : Node
{
    [Export] public EnemyStateMachine State;
    [Export] public Vector2 MovementVector;
    [Export] public float MoveSpeed = DefaultMoveSpeed; // Set as needed

    private Luce luce;
    public static readonly float DefaultMoveSpeed = 4000;
    public static readonly int MoveVelocityThreshold = 25; // might change

    private float MovementVectorThreshold => MoveVelocityThreshold / 100f;
    private bool StateIsValid => State.EnemyState is EnemyState.Attacking;

    public override void _Ready()
    {
        State ??= GetParent<EnemyStateMachine>();
    }

    private bool MovementVectorIsAboveThreshold(Vector2 vector) => vector.Length() > MovementVectorThreshold;

    public override void _PhysicsProcess(double delta)
    {
        if (StateIsValid) return;

        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);

        var movementVector = State.Enemy.Position.DirectionTo(luce.Position); 
        State.Enemy.CalculatedVelocity = MovementVectorIsAboveThreshold(movementVector) ? movementVector * MoveSpeed : Vector2.Zero;
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
