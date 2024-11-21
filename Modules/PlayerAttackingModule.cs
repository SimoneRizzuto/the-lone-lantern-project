using System;
using System.Linq;
using Godot;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Modules;

[GlobalClass]
public partial class PlayerAttackingModule : Node
{
    [Export] public PlayerStateMachine State;
    [Export] public CollisionPolygon2D AttackShape;

    private bool AllowAttack => State.StaminaHealthModule.AllowAction;
    
    private bool isBufferingNormalAttack;
    private bool isBufferingDashAttack;
    private int attackAnimationCounter = 1;
    private int AnimationFramesCount => State.MainSprite.SpriteFrames.GetFrameCount(State.MainSprite.Animation);
    private bool StateIsAttacking => State.PlayerState is PlayerState.Attacking;

    private AttackType attackTriggered = AttackType.None;
    
    public enum AttackType
    {
        None,
        Normal,
        Charge,
        Dash,
        Counter,
    }
    
    private Vector2 attackVector;
    
    public override void _Ready()
    {
        State ??= GetParent<PlayerStateMachine>();
        AttackShape ??= GetNode<CollisionPolygon2D>("HitBox/CollisionPolygon2D");
        
        State.MainSprite.AnimationFinished += OnAnimationFinished;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        /*if (State != PlayerState.Attacking)
        {
            //attackCount++;
            //Health -= 20; // DO NOT REMOVE, only uncomment when you want stamina to work

            //PauseStaminaRegen();
        }*/

        var dashAttackIsBuffered = isBufferingDashAttack && State.PlayerState != PlayerState.Dashing;
        if (dashAttackIsBuffered)
        {
            isBufferingDashAttack = false;
            TriggerDashAttack();
        }
    
        CheckAttackInput();
    
        if (!StateIsAttacking) return;

        if (isBufferingNormalAttack)
        {
            var onFinalFrame = AnimationFramesCount - 1 == State.MainSprite.Frame;
            if (onFinalFrame)
            {
                isBufferingNormalAttack = false;
                TriggerNormalAttack();
            }
        }

        ProcessAttack();
    }

    private void CheckAttackInput()
    {
        if (!Input.IsActionJustPressed(InputMapAction.Attack) || !AllowAttack) return;
        
        if (State.PlayerState == PlayerState.Attacking)
        {
            isBufferingNormalAttack = true;
        }
        else if (State.PlayerState == PlayerState.Dashing)
        {
            isBufferingDashAttack = true;
        }
        else
        {
            TriggerNormalAttack();
        }
    }
    
    public void TriggerNormalAttack()
    {
        attackVector = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);
        
        var direction = DirectionHelper.GetSnappedDirection(attackVector);
        if (direction != Direction.None)
        {
            State.LastDirection = direction;
        }
        
        if (Enum.GetName(State.LastDirection)?.ToLower() == "down") attackAnimationCounter = 1; // PLACEHOLDER to not break player from down attack, can remove once second down attack is added
        
        State.MainSprite.Play($"attack {Enum.GetName(State.LastDirection)?.ToLower()} {attackAnimationCounter}");
        
        if (attackAnimationCounter == 1)
        {
            attackAnimationCounter++;
        }
        else
        {
            attackAnimationCounter--;
        }
        
        State.Player.CalculatedVelocity = attackVector * 4000f;
        State.PlayerState = PlayerState.Attacking;
        
        attackTriggered = AttackType.Normal;
        State.StaminaHealthModule.RemoveStaminaHealth(20);
    }
    
    public void TriggerDashAttack()
    {
        Console.WriteLine("dash attack triggered...");
        TriggerNormalAttack(); // current placeholder
        // need to make a new dash attack for reaching far enemies, and/or for game feel.

        attackTriggered = AttackType.Dash;
        State.StaminaHealthModule.RemoveStaminaHealth(20);
        
        State.Player.CalculatedVelocity = attackVector * 6000f;
    }

    private void ProcessAttack()
    {
        switch (attackTriggered)
        {
            case AttackType.Normal:
                switch (State.MainSprite.Frame)
                {
                    case 1:
                        State.Player.CalculatedVelocity = attackVector * 1250f;
                        break;
                    case >= 2:
                        State.Player.CalculatedVelocity = attackVector;
                        AttackShape.Disabled = true;
                        break;
                }
                break;
            case AttackType.Dash:
                switch (State.MainSprite.Frame)
                {
                    case 1:
                        State.Player.CalculatedVelocity = attackVector * 5000f;
                        break;
                    case 2:
                        State.Player.CalculatedVelocity = attackVector * 3000f;
                        AttackShape.Disabled = true;
                        break;
                    case 3:
                        State.Player.CalculatedVelocity = attackVector * 2000f;
                        break;
                    case >= 4:
                        State.Player.CalculatedVelocity = attackVector;
                        break;
                }
                break;
        }
    }
    
    private void OnAnimationFinished()
    {
        State.PlayerState = PlayerState.Idle;
    }
}