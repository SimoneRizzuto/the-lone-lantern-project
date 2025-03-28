using System;
using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy;

[GlobalClass]
public partial class EnemyAttackingModule : Node
{
    [Export] public EnemyStateMachine State;
    [Export] public CollisionPolygon2D AttackShape;

    private Scripts.Player.Luce luce;

    private bool isBufferingNormalAttack;
    private int attackAnimationCounter = 1;
    //private int AnimationFramesCount => State.MainSprite.SpriteFrames.GetFrameCount(State.MainSprite.Animation);
    private bool StateIsAttacking => State.EnemyState is EnemyState.CombatAttack;

    private AttackType attackTriggered = AttackType.None;

    public enum AttackType
    {
        None,
        Normal,
    }

    private Vector2 attackVector;

    public override void _Ready()
    {
        State ??= GetParent<EnemyStateMachine>();
        AttackShape ??= GetNode<CollisionPolygon2D>("HitBox/CollisionPolygon2D");

        //State.MainSprite.AnimationFinished += OnAnimationFinished;
    }

    public override void _PhysicsProcess(double delta)
    {
        CheckAttackRecent();

        if (!StateIsAttacking) return;

        if (isBufferingNormalAttack)
        {
            /*var onFinalFrame = AnimationFramesCount - 1 == State.MainSprite.Frame;
            if (onFinalFrame)
            {
                isBufferingNormalAttack = false;
                TriggerNormalAttack();
            }*/
        }

        ProcessAttack();
    }

    public void TriggerNormalAttack()
    {
        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);

        attackVector = State.EnemyTemplate.Position.DirectionTo(luce.Position);

        var direction = DirectionHelper.GetSnappedDirection(attackVector);
        if (direction != Direction.None)
        {
            State.LastDirection = direction;
        }

        if (Enum.GetName(State.LastDirection)?.ToLower() == "down") attackAnimationCounter = 1; 

        //State.MainSprite.Play($"attack {Enum.GetName(State.LastDirection)?.ToLower()} {attackAnimationCounter}");

        if (attackAnimationCounter == 1)
        {
            attackAnimationCounter++; // change so just a single attack
        }
        else
        {
            attackAnimationCounter--;
        }

        State.EnemyTemplate.CalculatedVelocity = attackVector * 4000f;
        State.EnemyState = EnemyState.CombatAttack;

        attackTriggered = AttackType.Normal;
    }

    private void CheckAttackRecent()
    {
        // Need something here to mitigate the number of attacks otherwise will continue to attack (might be ok for now)
        //if () return;

        if (State.EnemyState == EnemyState.CombatAttack)
        {
            isBufferingNormalAttack = true;
        }
        else
        {
            TriggerNormalAttack();
        }
    }

    private void ProcessAttack()
    {
        switch (attackTriggered)
        {
            case AttackType.Normal:
                /*if (State.MainSprite.Frame == 1)
                {
                    State.EnemyTemplate.CalculatedVelocity = attackVector * 1250f;
                }
                else if (State.MainSprite.Frame >= 2)
                {
                    State.EnemyTemplate.CalculatedVelocity = attackVector;
                    AttackShape.Disabled = true;
                }*/
                break;
        }
    }

    private void OnAnimationFinished()
    {
        State.EnemyState = EnemyState.OutOfCombatIdle;
    }
}
