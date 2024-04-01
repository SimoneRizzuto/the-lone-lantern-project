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
    }
    public override void _PhysicsProcess(double delta)
    {
        FollowTarget(delta, playerTarget);
    }

    public virtual bool FollowTarget(double delta, PhysicsBody2D target)
    {
        // Calculate the direction and distance to the player
        var targetRange = GlobalPosition.DistanceTo(target.GlobalPosition);
        var targetDirection = GlobalPosition.DirectionTo(target.GlobalPosition);

        if (targetRange < AttackRange) return false;

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

    public virtual void Attack()
    {
        if (state == EnemyState.Attacking) return;
        
        
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