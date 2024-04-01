using Godot;
using System;
using System.Numerics;
using System.Text.Encodings.Web;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;
using TheLoneLanternProject.Scenes.Enemies.BaseNode;
using Vector2 = Godot.Vector2;

public partial class Player : CharacterBody2D
{
    [Signal] public delegate void HealthChangedEventHandler(double newHealth);

    [Export] public int Speed = 5000;

    private double health = 100;
    private double maxHealth = 100;
    private double regenSpeed = 0.35;
    private StaminaHealthState healthState;
    private Timer healthRegenBuffer;

    public Direction Direction = Direction.Right;
    public PlayerState State = PlayerState.Idle;

    private Vector2 vectorForMovement = Vector2.Zero;

    private AnimatedSprite2D mainSprite = new();
    private CollisionShape2D playerShape = new();
    private CollisionShape2D attackShape = new();
    private Timer attackTimer = new();

    private AnimatedSprite2D attackAnimation = new();

    public override void _Ready()
    {
        mainSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        attackAnimation = GetNode<AnimatedSprite2D>("AttackAnimation");

        playerShape = GetNode<CollisionShape2D>("PlayerShape");
        attackShape = GetNode<CollisionShape2D>("AttackShape/CollisionShape2D");

        attackTimer = GetNode<Timer>("Timers/AttackTimer");
        healthRegenBuffer = GetNode<Timer>("Timers/HealthRegenBuffer");
    }

    public override void _Process(double delta)
    {
        SetDirection();
        SetPlayerState();
        SetFlipH();
        SetAnimation();

        RegenerateHealth();
    }
    public override void _PhysicsProcess(double delta)
    {
        if (State != PlayerState.Attacking)
        {
            vectorForMovement = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);

            State = vectorForMovement != Vector2.Zero ? PlayerState.Walking : PlayerState.Idle;
        }
        Velocity = vectorForMovement * Speed * (float)delta;

        
        /*
        var tween = CreateTween().SetEase(Tween.EaseType.Out);

        if (State == PlayerState.Attack)
        {
            tween.TweenProperty(this, "velocity", vectorForMovement * Speed * (float)delta, 0.2f);
        }
        else
        {
            tween.Stop();

            vectorForMovement = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);
            Velocity = vectorForMovement * Speed * (float)delta;

            if (vectorForMovement != Vector2.Zero)
            {
                State = PlayerState.Walk;
            }
            else
            {
                State = PlayerState.Idle;
            }
        }
        */
        
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
        if (Input.IsActionJustPressed(InputMapAction.Attack) && State != PlayerState.Disabled)
        {
            if (State != PlayerState.Attacking && health > 0)
            {
                State = PlayerState.Attacking;
                attackTimer.Start();

                health -= 20;
                healthRegenBuffer.Start();
                healthState = StaminaHealthState.Pause;
                EmitSignal(SignalName.HealthChanged, health);
            }
        }
    }

    private void SetFlipH()
    {
        if (State == PlayerState.Attacking) return;
        
        if (Direction == Direction.Left)
        {
            var wasFacingRight = !mainSprite.FlipH;

            mainSprite.FlipH = true;
            attackAnimation.FlipH = true;

            if (wasFacingRight)
            {
                attackShape.Position = attackShape.Position.Reflect(Vector2.Up);
                attackAnimation.Offset = attackAnimation.Offset.Reflect(Vector2.Up);
            }
        }
        else if (Direction == Direction.Right)
        {
            var wasFacingLeft = mainSprite.FlipH;

            mainSprite.FlipH = false;
            attackAnimation.FlipH = false;

            if (wasFacingLeft)
            {
                attackShape.Position = attackShape.Position.Reflect(Vector2.Up);
                attackAnimation.Offset = attackAnimation.Offset.Reflect(Vector2.Up);
            }
        }
    }

    private void SetAnimation()
    {
        if (State == PlayerState.Idle)
        {
            mainSprite.Animation = "idle";
        }
        else if (State == PlayerState.Walking)
        {
            mainSprite.Animation = "walk";
            mainSprite.Play();
        }
        else if (State == PlayerState.Attacking)
        {
            mainSprite.Animation = "attack";
            mainSprite.Play();
            
            if (mainSprite.Frame == 1)
            {
                attackShape.Disabled = false;
                attackAnimation.Visible = true;
                attackAnimation.Play();
            }

            if (mainSprite.Frame == 3)
            {
                attackShape.Disabled = true;
                attackAnimation.Visible = false;
                attackAnimation.Stop();
                State = PlayerState.Walking;
            }
        }
    }

    private void RegenerateHealth()
    {
        if (healthState != StaminaHealthState.Regen) return;
        
        if (health < maxHealth)
        {
            health += regenSpeed;
            EmitSignal(SignalName.HealthChanged, health);
        }
        else
        {
            healthState = StaminaHealthState.Max;
        }
    }

    // Signal Events
    public void OnAttackTimerTimeout()
    {
        State = PlayerState.Idle;
    }

    public void OnHealthRegenBufferTimeout()
    {
        healthState = StaminaHealthState.Regen;
    }
    public void OnAttackShapeAreaEntered(Node2D area)
    {
        GD.Print("Area Entered: Attack");
        if (area.IsInGroup(NodeGroup.Enemy))
        {
            var enemy = (EnemyBase)area;
            enemy.TakeDamage(1);
        }

    }
}
