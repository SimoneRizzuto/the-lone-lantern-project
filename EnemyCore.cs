using Godot;
using System.Linq;
using TheLoneLanternProject.Constants;

namespace TheLoneLanternProject;
public interface IEnemy
{
    bool FollowTarget(double delta, PhysicsBody2D target);
    void Die();
    void TakeDamage(int damage);
}

public abstract partial class EnemyCore : Area2D, IEnemy
{
    [Export] public int Speed = 50;
    [Export] public int Health = 2;
    [Export] public PathFollow2D PathToFollow = new();

    private CharacterBody2D playerTarget = new();
    
    public override void _Ready()
    {
        // Add enemy on all loops at once
        
        var enemyPath = GetNodeOrNull("./EnemyPaths");
        if (enemyPath != null)
        {
            var pathChildren = enemyPath.GetChildren();

            var pathToFollow = pathChildren.FirstOrDefault();
            if (pathToFollow != null)
            {
                pathToFollow.AddChild(PathToFollow);
            }
        }

        PathToFollow.Loop = false;

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

    public void RemoveEnemyFromPath()
    {
        var paths = GetNodeOrNull("./EnemyPaths"); // Hard code for testing
        if (paths?.GetChildCount() > 0)
        {
            var path1 = GetNodeOrNull<Path2D>("./EnemyPaths/Path1");
            if (path1 != null)
            {
                paths.RemoveChild(path1);
            }
        }
    }

    public virtual bool FollowTarget(double delta, PhysicsBody2D target)
    {
        // Calculate the direction and distance to the player
        var distance = GlobalPosition.DistanceTo(target.GlobalPosition);
        var direction = GlobalPosition.DirectionTo(target.GlobalPosition);

        if (distance < 25) return false;

        if (distance < 100)
        {
            // Break path and head directly for the player
            // Add in move and slide if collide with building
            RemoveEnemyFromPath();
            Position += direction * Speed * (float)delta;

            return true;
        }

        PathToFollow.Progress += Speed * (float)delta;
        if (PathToFollow != null)
        {
            Position = PathToFollow.Position;
        }

        if (PathToFollow.ProgressRatio >= 1)
        {
            QueueFree();
        }

        return false;
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
        // Keeping the code here for now, but I am not convinces that the commented code is needed, since the node will de-spawn anyway
        /*
        Speed = 0;
        GetNode<CollisionShape2D>("./CollisionShape2D").SetDeferred("disabled", true);
        GetNode<Sprite2D>("./Sprite2D").Hide();
        */

        QueueFree();
    }
}
