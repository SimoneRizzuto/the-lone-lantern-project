using System;
using System.Linq;
using Godot;
using TheLoneLanternProject.Constants;

namespace TheLoneLanternProject.Scenes.Enemies.BaseNode;
public interface IEnemy
{
    bool FollowTarget(double delta, PhysicsBody2D target);
    void Die();
    void TakeDamage(int damage);
}

public partial class EnemyBase : Area2D, IEnemy
{
    [Export] public int Health = 1;
    [Export] public int MoveSpeed = 50;
    [Export] public int AttackRange = 50;

    [Export] public PathFollow2D PathToFollow = new();
    [Export] public bool LoopInPath = true;

    private CharacterBody2D playerTarget = new();

    private EnemyState state = EnemyState.Default;
    private Direction direction = Direction.Right;

    private AnimatedSprite2D characterSprite = new();

    public override void _Ready()
    {
        // Add enemy on all loops at once
        var enemyPath = GetNodeOrNull<PathFollow2D>("TestPath/PathFollow");
        if (enemyPath != null)
        {
            PathToFollow = enemyPath;
            PathToFollow.Loop = LoopInPath;
        }

        var playerNode = GetTree().GetNodesInGroup(NodeGroup.Player).FirstOrDefault();
        if (playerNode is CharacterBody2D characterBody2D)
        {
            playerTarget = characterBody2D;
        }

        var spriteToFetch = GetNodeOrNull("AnimatedSprite2D");
        if (spriteToFetch is AnimatedSprite2D animatedSprite2D)
        {
            characterSprite = animatedSprite2D;
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        FollowTarget(delta, playerTarget);

        if (state == EnemyState.Attacking)
        {
            ProcessAttack();
        }
    }

    public virtual bool FollowTarget(double delta, PhysicsBody2D target)
    {
        // Calculate the direction and distance to the player
        var targetRange = GlobalPosition.DistanceTo(target.GlobalPosition);
        var targetDirection = GlobalPosition.DirectionTo(target.GlobalPosition);

        CalculateDirection(targetDirection);
        
        if (targetRange < AttackRange)
        {
            state = EnemyState.Attacking;
            return false;
        }

        if (targetRange < 100)
        {
            // Break path and head directly for the player
            // Add in move and slide if collide with building
            Position += targetDirection * MoveSpeed * (float)delta;
            return true;
        }

        if (PathToFollow != null)
        {
            PathToFollow.Progress += MoveSpeed * (float)delta;
            Position = PathToFollow.Position;
            if (PathToFollow.ProgressRatio >= 1)
            {
                QueueFree();
            }
        }

        return false;
    }

    private void ProcessAttack()
    {
        
    }
    private void CalculateDirection(Vector2 targetDirection)
    {
        if (targetDirection.X > 0)
        {
            direction = Direction.Right;
            characterSprite.FlipH = false;
        }
        else if (targetDirection.X < 0)
        {
            direction = Direction.Left;
            characterSprite.FlipH = true;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
    
    public virtual void Die()
    {
        QueueFree();
    }
    
    // SIGNALS

}

public enum EnemyState
{
    Default,
    Following,
    Attacking,
    Hurting,
    Disabled
}