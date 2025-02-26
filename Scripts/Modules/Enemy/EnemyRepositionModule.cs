using System;
using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;
//using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy;

[GlobalClass]
public partial class EnemyRepositionModule : Node
{
    [Export] public EnemyStateMachine State;
    [Export] public Vector2 MovementVector;
    [Export] public float MoveSpeed = DefaultMoveSpeed; // Set as needed

    private Scripts.Player.Luce luce;
    public static readonly float DefaultMoveSpeed = 4000;
    public static readonly int MoveVelocityThreshold = 25; // might change
    public static readonly int PauseDistanceThreshold = 75;

    private float MovementVectorThreshold => MoveVelocityThreshold; // / 100f;
    private bool StateIsValid => State.EnemyState is EnemyState.Attacking;
    private bool PausedOnce = false;

    public override void _Ready()
    {
        State ??= GetParent<EnemyStateMachine>();
    }

    private bool MovementVectorIsAboveThreshold(Vector2 vector) => vector.Length() > MovementVectorThreshold;

    public override void _PhysicsProcess(double delta)
    {
        //GD.Print(State.EnemyState);
        if (StateIsValid) return;

        
        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);

        var movementVector = State.Enemy.Position.DirectionTo(luce.Position);
        var distance = State.Enemy.Position.DistanceTo(luce.Position);

        if ((State.EnemyState is EnemyState.OutOfCombat) | (State.EnemyState is EnemyState.Attacking)){
            return;
        }
            
        else if (distance >= PauseDistanceThreshold)
        {
            State.Enemy.CalculatedVelocity = movementVector * MoveSpeed;

        }
        else if ((distance <= PauseDistanceThreshold) & (PausedOnce is true))
        {
            TransitionToAttack();
        }
        else if (distance <= PauseDistanceThreshold)
        { 
            if (PausedOnce is true) return;
            State.Enemy.CalculatedVelocity = movementVector.Orthogonal() * MoveSpeed;
            TransitionToAttack();
            
        }
        
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
        }
        

        State.MainSprite.Play();
    }

    public async void TransitionToAttack()
    {
        if (PausedOnce is false)
        {
            await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
            State.Enemy.CalculatedVelocity = Vector2.Zero;
            //await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
            PausedOnce = true;
        }
        else
        {
            State.EnemyState = EnemyState.Attacking;
        }
        
        
        
    }
}
