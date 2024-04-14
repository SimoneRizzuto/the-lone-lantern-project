using System;
using Godot;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Enemies.BaseNode;
using Vector2 = Godot.Vector2;

namespace TheLoneLanternProject.Scenes.Player;

public partial class Player : CharacterBody2D
{
    [Signal] public delegate void HealthChangedEventHandler(double newHealth);

    [Export] public int Speed = 5000;

    private int attackMoveSpeed = 8000;

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
    private CollisionPolygon2D attackShape = new();
    private Timer attackTimer = new();

    public override void _Ready()
    {
        mainSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        playerShape = GetNode<CollisionShape2D>("PlayerShape");
        attackShape = GetNode<CollisionPolygon2D>("HitBox/CollisionPolygon2D");

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

        Speed = State == PlayerState.Attacking ? attackMoveSpeed : 5000;
        
        Velocity = vectorForMovement * Speed * (float)delta;
        
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
        else if (Input.IsActionPressed(InputMapAction.Up))
        {
            Direction = Direction.Up;
        }
        else if (Input.IsActionPressed(InputMapAction.Down))
        {
            Direction = Direction.Down;
        }
    }

    private void SetPlayerState()
    {
        if (Input.IsActionJustPressed(InputMapAction.Attack) && State != PlayerState.Disabled)
        {
            if (health > 0 && !disableAttackingInput)
            {
                if (internalAttackCounter == 1)
                {
                    internalAttackCounter++;
                }
                else
                {
                    internalAttackCounter--;
                }
                State = PlayerState.Attacking;
                attackTimer.Start();

                health -= 20;
                healthRegenBuffer.Start();
                healthState = StaminaHealthState.Pause;
                EmitSignal(SignalName.HealthChanged, health);
            }
        }
    }


    private bool disableAttackingInput;
    private void SetFlipH()
    {
        if (State == PlayerState.Attacking) return;

        switch (Direction)
        {
            case Direction.Left:
            {
                var wasFacingRight = !mainSprite.FlipH;

                mainSprite.FlipH = true;

                if (wasFacingRight)
                {
                    attackShape.Scale = new Vector2(-1, 1);
                    //attackShape.Position = attackShape.Position.Reflect(Vector2.Up);
                }

                break;
            }
            case Direction.Right:
            {
                var wasFacingLeft = mainSprite.FlipH;

                mainSprite.FlipH = false;

                if (wasFacingLeft)
                {
                    attackShape.Scale = new Vector2(1, 1);
                    //attackShape.Position = attackShape.Position.Reflect(Vector2.Up);
                }

                break;
            }
        }
    }

    private int internalAttackCounter = 2;
    
    private void SetAnimation()
    {
        var animationDirection = "";
        switch (Direction)
        {
            case Direction.Left:  animationDirection = "side"; break;
            case Direction.Right: animationDirection = "side"; break;
            case Direction.Down:  animationDirection = "down"; break;
            case Direction.Up:    animationDirection = "up";   break;
        }
        
        if (State == PlayerState.Idle)
        {
            mainSprite.Animation = $"idle {animationDirection}";
        }
        else if (State == PlayerState.Walking)
        {
            mainSprite.Animation = $"walk {animationDirection}";
            mainSprite.Play();
        }
        else if (State == PlayerState.Attacking)
        {
            mainSprite.Animation = $"attack {animationDirection} {internalAttackCounter}";
            mainSprite.Play();

            disableAttackingInput = true;
            
            if (mainSprite.Frame == 0)
            {
                attackShape.Disabled = false;
                attackMoveSpeed = 4000;
            }
            else if (mainSprite.Frame == 1)
            {
                attackMoveSpeed = 1250;
            }
            else
            {
                attackMoveSpeed = 0;
                attackShape.Disabled = true;
                disableAttackingInput = false;
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
        //State = PlayerState.Idle;
    }

    private void OnAnimationFinished()
    {
        if (mainSprite.Animation.ToString().Contains("attack"))
        {
            State = PlayerState.Idle;
            internalAttackCounter = 2;
        }
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