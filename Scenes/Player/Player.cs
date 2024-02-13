using Godot;
using System;
using TheLoneLanternProject.Constants;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed = 5000;

    public Direction CurrentDirection = Direction.Right;
    public PlayerState State = PlayerState.Idle;

    private AnimatedSprite2D _sprite = new();
    private CollisionShape2D _collisionShape = new();
    
    
    public enum Direction
    {
        Left,
        Right
    }
    public enum PlayerState
    {
        Idle,
        Walk,
        Attack,
        Hurt,
        Disabled
    }

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public override void _Process(double delta)
    {
        SetDirection();

        if (State == PlayerState.Idle)
        {
            _sprite.Animation = "idle";
        }
        else if (State == PlayerState.Walk)
        {
            _sprite.Animation = "walk";
            _sprite.Play();
        }

        if (CurrentDirection == Direction.Left)
        {
            _sprite.FlipH = true;
        }
        else if (CurrentDirection == Direction.Right)
        {
            _sprite.FlipH = false;
        }
    }

    private void SetDirection()
    {
        if (Input.IsActionPressed(InputMapAction.Left))
        {
            CurrentDirection = Direction.Left;
        }
        else if (Input.IsActionPressed(InputMapAction.Right))
        {
            CurrentDirection = Direction.Right;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var vector = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);
        var position = vector * Speed * (float)delta;
        
        Velocity = position;

        if (vector != Vector2.Zero)
        {
            State = PlayerState.Walk;
        }
        else
        {
            State = PlayerState.Idle;
        }
        
        MoveAndSlide();
    }
}
