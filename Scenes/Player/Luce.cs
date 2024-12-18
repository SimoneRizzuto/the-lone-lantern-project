using Godot;
using System;
using Vector2 = Godot.Vector2;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Extensions;
using TheLoneLanternProject.Scenes.Enemies.BaseNode;
using TheLoneLanternProject.Helpers;

namespace TheLoneLanternProject.Scenes.Player;
public partial class Luce : CharacterBody2D
{
    [Signal] public delegate void HealthChangedEventHandler(double newHealth);
    [Signal] public delegate void LastWalkDirectionEventHandler(int direction);
    [Signal] public delegate void PlayerIsMovingEventHandler(bool isMoving);
    
    public static int MoveSpeed = 5000;
    public static readonly int MoveVelocityThreshold = 25;
    
    private bool VelocityIsAboveThreshold(float x, float y) => Math.Abs(x) > MoveVelocityThreshold || Math.Abs(y) > MoveVelocityThreshold;
    
    private CustomSignals customSignals = new();
    public AudioDirector audioDirector;
    
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
    private CollisionShape2D mainShape = new();
    private CollisionPolygon2D attackShape = new();
    

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        mainSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        mainShape = GetNode<CollisionShape2D>("MainShape");
        attackShape = GetNode<CollisionPolygon2D>("HitBox/CollisionPolygon2D");

        healthRegenBuffer = GetNode<Timer>("Timers/HealthRegenBuffer");

        

        audioDirector = AudioDirector.Instance;
    }

    public override void _Process(double delta)
    {
        if (State == PlayerState.Disabled) return;

        SetDirection();
        SetAttack();
        SetAnimation();

        RegenerateHealth();
        
    }
    public override void _PhysicsProcess(double delta)
    {

        if (State == PlayerState.Disabled) return;
        
        if (State != PlayerState.Attacking)
        {
            vectorForMovement = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);
            
            State = VelocityIsAboveThreshold(vectorForMovement.X * 100, vectorForMovement.Y * 100) ? PlayerState.Walking : PlayerState.Idle;
        }
        
        MoveSpeed = State == PlayerState.Attacking ? attackMoveSpeed : MoveSpeed;
        
        if (VelocityIsAboveThreshold(vectorForMovement.X * 100, vectorForMovement.Y * 100))
        {
            var calculatedVelocity = vectorForMovement * MoveSpeed * (float)delta;
            Velocity = calculatedVelocity;
            
            MoveAndSlide();
        }
        
        Position = Position.RoundToNearestValue(0.25);
    }
    
    private void SetDirection()
    {
        var setDirection = HeldDirection();

        if (Input.IsActionJustPressed("TestTriggerDialogue")) // TEST DIALOGUE TRIGGER FOR NOW, DELETE LATER
        {
            customSignals.EmitSignal(nameof(CustomSignals.ShowDialogueBalloon), "dialogue-test", "initial_dialogue");
        }

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

    private int attackAnimationCounter = 2;
    private void SetAnimation()
    {
        var animationDirection = "";
        
        switch (Direction)
        {
            case Direction.Left:  animationDirection = "left"; break;
            case Direction.Right: animationDirection = "right"; break;
            case Direction.Down:  animationDirection = "down"; break;
            case Direction.Up:    animationDirection = "up";   break;
        }
        
        if (State == PlayerState.Idle)
        {
            mainSprite.Animation = $"idle {animationDirection}";
            EmitSignal(SignalName.LastWalkDirection, (int)nextBuffer.NextDirection);
            EmitSignal(SignalName.PlayerIsMoving, false);
        }
        else if (State == PlayerState.Walking)
        {
            mainSprite.Animation = $"walk {animationDirection}";
            EmitSignal(SignalName.LastWalkDirection, (int)nextBuffer.NextDirection);
            EmitSignal(SignalName.PlayerIsMoving, true);
            mainSprite.Play();
        }
        else if (State == PlayerState.Attacking)
        {
            EmitSignal(SignalName.PlayerIsMoving, true);
            
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
        if (State == PlayerState.Disabled) return;
        
        if (mainSprite.Animation.ToString().Contains("attack"))
        {
            State = PlayerState.Idle;
            attackAnimationCounter = 2;
        }
    }

    public void OnHealthRegenBufferTimeout()
    {
        if (State == PlayerState.Disabled) return;
        
        healthState = StaminaHealthState.Regen;
    }

    public void OnAttackShapeAreaEntered(Node2D area)
    {
        if (State == PlayerState.Disabled) return;
        
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