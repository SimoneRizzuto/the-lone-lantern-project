using Godot;
using System;
using System.Linq;
using TheLoneLanternProject;
using TheLoneLanternProject.Constants;

public partial class Enemy : Area2D, IEnemy
{
    [Export]
    public int Speed = 50;

    public int Health = 2;
    
    // Create Path object
    public PathFollow2D Follow = new();

    // Create the player as a target
    public CharacterBody2D PlayerTarget = new();

    public override void _Ready()
    {
        // Add enemy on all loops at once
        var enemyPath = GetNode<Node>("./EnemyPaths");
        var pathChildren = enemyPath.GetChildren();

        var pathToFollow = pathChildren.FirstOrDefault();
        if (pathToFollow != null)
        {
            pathToFollow.AddChild(Follow);
        }

        Follow.Loop = false;

        var playerNode = GetTree().GetNodesInGroup(NodeGroup.Player).FirstOrDefault();
        if (playerNode is CharacterBody2D characterBody2D)
        {
            PlayerTarget = characterBody2D;
        }
    }

    public void RemoveEnemyFromPath()
    {
        var paths = GetNode<Node>("./EnemyPaths"); // Hard code for testing
        if (paths.GetChildCount() > 0 )
        {
            paths.RemoveChild(GetNode<Path2D>("./EnemyPaths/Path1"));
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        FollowTarget(delta, PlayerTarget);
    }

    public void FollowTarget(double delta, PhysicsBody2D target)
    {
        // Calculate the direction and distance to the player
        var distance = GlobalPosition.DistanceTo(target.GlobalPosition);
        var direction = GlobalPosition.DirectionTo(target.GlobalPosition);

        if (distance < 25) return;

        if (distance < 100)
        {
            // Break path and head directly for the player
            // Add in move and slide if collide with building
            RemoveEnemyFromPath();
            Position += direction * Speed * (float)delta;
        }
        else
        {
            Follow.Progress += Speed * (float)delta;
            Position = Follow.Position;
            if (Follow.ProgressRatio >= 1)
            {
                QueueFree();
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void FollowTarget()
    {
        throw new NotImplementedException();
    }

    public void Die()
    {
        Speed = 0;
        GetNode<CollisionShape2D>("./CollisionShape2D").SetDeferred("disabled", true);
        GetNode<Sprite2D>("./Sprite2D").Hide();
        QueueFree();
    }

    // SIGNALS
    public void OnAreaEntered(Node2D area)
    {
        if (area.IsInGroup("attack"))
        {
            Enemy enemy = (Enemy)area;
            enemy.TakeDamage(1);
        }
    }
}
