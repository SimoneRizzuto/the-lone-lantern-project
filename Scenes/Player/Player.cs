using Godot;
using System;
using System.Numerics;
using System.Text.Encodings.Web;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;
using Vector2 = Godot.Vector2;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed = 5000;

    public Direction Direction = Direction.Right;
    public PlayerState State = PlayerState.Idle;

    private AnimatedSprite2D _sprite = new();
    private CollisionShape2D _collisionShape = new();
    private CollisionShape2D _attackShape = new();
    private Timer _attackTimer = new();

    private AnimatedSprite2D _attackAnimation = new();

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _attackTimer = GetNode<Timer>("AttackTimer");
        
        _attackShape = GetNode<CollisionShape2D>("AttackShape2D");
        _attackAnimation = GetNode<AnimatedSprite2D>("AttackAnimation");
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
            var wasFacingRight = !_sprite.FlipH;

            _sprite.FlipH = true;
            _attackAnimation.FlipH = true;

            if (wasFacingRight)
            {
                _attackShape.Position = _attackShape.Position.Reflect(Vector2.Up);
                _attackAnimation.Offset = _attackAnimation.Offset.Reflect(Vector2.Up);
            }
        }
        else if (Direction == Direction.Right)
        {
            var wasFacingLeft = _sprite.FlipH;

            _sprite.FlipH = false;
            _attackAnimation.FlipH = false;

            if (wasFacingLeft)
            {
                _attackShape.Position = _attackShape.Position.Reflect(Vector2.Up);
                _attackAnimation.Offset = _attackAnimation.Offset.Reflect(Vector2.Up);
            }
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
            
            if (_sprite.Frame == 1)
            {
                _attackShape.Disabled = false;
                _attackAnimation.Visible = true;
                _attackAnimation.Play();
            }

            if (_sprite.Frame == 3)
            {
                _attackShape.Disabled = true;
                _attackAnimation.Visible = false;
                _attackAnimation.Stop();
            }
        }
    }

    
    // Signal Events
    public void OnAttackTimerTimeout()
    {
        State = PlayerState.Idle;
    }
}
