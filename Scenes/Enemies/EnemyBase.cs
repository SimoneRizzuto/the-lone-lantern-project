using Godot;
using System.Linq;
using TheLoneLanternProject.Constants;

namespace TheLoneLanternProject.Scenes.Enemies;
public interface IEnemy
{
    bool FollowTarget(double delta, PhysicsBody2D target);
    void Die();
    void TakeDamage(int damage);
}

public abstract partial class EnemyBase : Area2D, IEnemy
{
    [Export] public int Speed = 50;
    [Export] public int Health = 1;
    [Export] public PathFollow2D PathToFollow = new();

    private CharacterBody2D playerTarget = new();
    
    public override void _Ready()
    {
        // Add enemy on all loops at once
        var enemyPath = GetNodeOrNull<PathFollow2D>("TestPath/PathFollow");
        if (enemyPath != null)
        {
            PathToFollow = enemyPath;
            PathToFollow.Loop = false;
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
        var targetDistance = GlobalPosition.DistanceTo(target.GlobalPosition);
        var targetDirection = GlobalPosition.DirectionTo(target.GlobalPosition);

        if (targetDistance < 25) return false;

        if (targetDistance < 100)
        {
            // Break path and head directly for the player
            // Add in move and slide if collide with building
            Position += targetDirection * Speed * (float)delta;

            return true;
        }

        if (PathToFollow != null)
        {
            PathToFollow.Progress += Speed * (float)delta;
            Position = PathToFollow.Position;
            if (PathToFollow.ProgressRatio >= 1)
            {
                QueueFree();
            }
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
    
    // SIGNALS

    /// <summary>
    /// Called when Area enters an Area2D.
    /// </summary>
    /// <param name="area"></param>
    public virtual void OnAreaEntered(Node2D area)
    {
        if (area.IsInGroup(NodeGroup.Attack))
        {
            TakeDamage(1);
        }
    }
}
