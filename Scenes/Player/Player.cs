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

    private int attackMoveSpeed = 4000;

    private double health = 100;
    public double Health
    {
        get => health;
        set
        {
            health = value;
            EmitSignal(SignalName.HealthChanged, health);

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private double maxHealth = 100;
    private double regenSpeed = 0.35;
    private StaminaHealthState healthState;
    private Timer healthRegenBuffer;
    
    private int attackCount;
    private PlayerNextBuffer nextBuffer = new();
    //private bool disableAttackingInput;

    public Direction Direction = Direction.Down;
    public PlayerState State = PlayerState.Idle;

    private Vector2 vectorForMovement = Vector2.Zero;

    private AnimatedSprite2D mainSprite = new();
    private CollisionShape2D playerShape = new();
    private CollisionPolygon2D attackShape = new();

    public override void _Ready()
    {
        mainSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        playerShape = GetNode<CollisionShape2D>("PlayerShape");
        attackShape = GetNode<CollisionPolygon2D>("HitBox/CollisionPolygon2D");

        healthRegenBuffer = GetNode<Timer>("Timers/HealthRegenBuffer");
    }

    public override void _Process(double delta)
    {
        if (State == PlayerState.Disabled) return;

        SetDirection();
        SetAttack();
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
        var setDirection = HeldDirection();

        nextBuffer.NextDirection = setDirection ?? Direction;
        
        if (State != PlayerState.Attacking)
        {
            Direction = setDirection ?? Direction;
        }
    }

    private void SetAttack()
    {
        if (!Input.IsActionJustPressed(InputMapAction.Attack)) return;
        
        if (Health > 0)
        {
            if (!nextBuffer.IsBuffering)
            {
                if (attackAnimationCounter == 1)
                {
                    attackAnimationCounter++;
                }
                else
                {
                    attackAnimationCounter--;
                }
            }
            
            if (State == PlayerState.Attacking)
            {
                nextBuffer.IsBuffering = true;
            }
            else
            {
                attackCount++;
                Health -= 20; // DO NOT REMOVE, only uncomment when you want stamina to work
                
                PauseStaminaRegen();
            }
            
            State = PlayerState.Attacking;
        }
    }
    
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
                }

                break;
            }
            case Direction.Down:
            {
                mainSprite.FlipH = false;
                
                if (attackAnimationCounter % 2 == 0) // is even
                {
                    mainSprite.FlipH = true;
                }
                
                break;
            }
        }
    }

    private int attackAnimationCounter = 2;
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
            if (nextBuffer.IsBuffering)
            {
                if (mainSprite.Frame == 3)
                {
                    Direction = nextBuffer.NextDirection;
                    mainSprite.Frame = 0;
                    nextBuffer.IsBuffering = false;
                }
            }
            else
            {
                if (Direction is Direction.Left or Direction.Right)
                {
                    mainSprite.Animation = $"attack {animationDirection} {attackAnimationCounter}";
                }
                else
                {
                    mainSprite.Animation = $"attack {animationDirection}";
                }
            }
            
            mainSprite.Play();
            
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
            }
        }
    }

    private void RegenerateHealth()
    {
        if (healthState != StaminaHealthState.Regen) return;
        
        if (Health < maxHealth)
        {
            Health += regenSpeed;
        }
        else
        {
            healthState = StaminaHealthState.Max;
        }
    }

    private Direction? HeldDirection()
    {
        if (Input.IsActionPressed(InputMapAction.Up)) return Direction.Up;
        if (Input.IsActionPressed(InputMapAction.Down)) return Direction.Down;
        if (Input.IsActionPressed(InputMapAction.Right)) return Direction.Right;
        if (Input.IsActionPressed(InputMapAction.Left)) return Direction.Left;
        return null;
    }

    private void PauseStaminaRegen()
    {
        healthRegenBuffer.Start();
        healthState = StaminaHealthState.Pause;
        EmitSignal(SignalName.HealthChanged, Health);
    }

    public void TakeDamage(double amount)
    {
        Health -= amount;
    }

    private void Die()
    {
        
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
            attackAnimationCounter = 2;
        }
    }

    public void OnHealthRegenBufferTimeout()
    {
        healthState = StaminaHealthState.Regen;
    }

    public void OnAttackShapeAreaEntered(Node2D area)
    {
        if (area.IsInGroup(NodeGroup.Enemy)) 
        {
            var enemy = (EnemyBase)area;
            enemy.TakeDamage(1);
        }
    }   
}

public class PlayerNextBuffer
{
    public bool IsBuffering { get; set; }
    public Direction NextDirection { get; set; }
    
}