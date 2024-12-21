using System;
using Godot;
using TheLoneLanternProject.Scripts.Helpers;
using TheLoneLanternProject.Scripts.Constants;
using TheLoneLanternProject.Scripts.StateMachines.Player;

namespace TheLoneLanternProject.Scripts.Modules.Player;

[GlobalClass]
public partial class PlayerWalkingModule : Node
{
    [Signal] public delegate void LastWalkDirectionEventHandler(int direction);
    [Signal] public delegate void PlayerIsMovingEventHandler(bool isMoving);
    
    [Export] public PlayerStateMachine State;
    
    [Export] public Vector2 MovementVector;
    [Export] public float MoveSpeed = DefaultMoveSpeed;
    
    public static readonly float DefaultMoveSpeed = 5000;
    public static readonly int MoveVelocityThreshold = 25;
    
    private float CurrentMoveSpeed => State.StaminaHealthModule.CurrentStaminaHealth <= 5 ? MoveSpeed / 3 : MoveSpeed;
    private float MovementVectorThreshold => MoveVelocityThreshold / 100f;
    private bool StateIsValid => State.PlayerState is PlayerState.Attacking or PlayerState.Dashing or PlayerState.Disabled;

    public override void _Ready()
    {
        State ??= GetParent<PlayerStateMachine>();
    }
    
    private bool MovementVectorIsAboveThreshold(Vector2 vector) => vector.Length() > MovementVectorThreshold;

    public override void _PhysicsProcess(double delta)
    {
        if (StateIsValid) return;
        
        var movementVector = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);
        State.Player.CalculatedVelocity = MovementVectorIsAboveThreshold(movementVector) ? movementVector * CurrentMoveSpeed : Vector2.Zero;
        
        SetMovementAnimation(movementVector);
    }
    
    private void SetMovementAnimation(Vector2 movementVector)
    {
        var walkDirection = DirectionHelper.GetSnappedDirection(movementVector, MovementVectorThreshold);
        
        var isWalking = walkDirection != Direction.None;
        if (isWalking)
        {
            var animation = $"walk {Enum.GetName(walkDirection)?.ToLower()}";
            var speed = Mathf.Snapped(movementVector.Length(), MovementVectorThreshold * 2);
            State.MainSprite.Play(animation, speed);
            
            State.LastDirection = walkDirection;
            State.PlayerState = PlayerState.Walking;
            
            EmitSignal(SignalName.LastWalkDirection, (int)walkDirection);
        }
        else
        {
            State.PlayerState = PlayerState.Idle;
        }
        
        EmitSignal(SignalName.PlayerIsMoving, isWalking);
        State.MainSprite.Play();
    }
}