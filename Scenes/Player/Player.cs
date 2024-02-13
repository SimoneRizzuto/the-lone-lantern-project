using Godot;
using System;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed = 5000;

    public Direction Direction = Direction.Right;
    public PlayerState State = PlayerState.Idle;

    private AnimatedSprite2D _sprite = new();
    private CollisionShape2D _collisionShape = new();
    private Timer _attackTimer = new();

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _attackTimer = GetNode<Timer>("AttackTimer");
    }

    public override void _Process(double delta)
    {
        SetDirection();
        SetPlayerState();
        SetFlipH();
        SetAnimation();
    }
    public override void _PhysicsProcess(double delta)
    {
        if (State == PlayerState.Attack) return;
        
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

    private void SetDirection()
    {
        if (Input.IsActionPressed(InputMapAction.Left))
        {
            Direction = Direction.Left;
        }
        else if (Input.IsActionPressed(InputMapAction.Right))
        {
            Direction = Direction.Right;
        }
    }

    private void SetPlayerState()
    {
        if (Input.IsActionPressed(InputMapAction.Attack))
        {
            State = PlayerState.Attack;
            _attackTimer.Start();
        }
    }

    private void SetFlipH()
    {
        if (State == PlayerState.Attack) return;
        
        if (Direction == Direction.Left)
        {
            _sprite.FlipH = true;
        }
        else if (Direction == Direction.Right)
        {
            _sprite.FlipH = false;
        }
    }

    private void SetAnimation()
    {
        if (State == PlayerState.Idle)
        {
            _sprite.Animation = "idle";
        }
        else if (State == PlayerState.Walk)
        {
            _sprite.Animation = "walk";
            _sprite.Play();
        }
        else if (State == PlayerState.Attack)
        {
            _sprite.Animation = "attack";
            _sprite.Play();
        }
    }

    
    // Signal Events
    public void OnAttackTimerTimeout()
    {
        State = PlayerState.Idle;
    }
}
