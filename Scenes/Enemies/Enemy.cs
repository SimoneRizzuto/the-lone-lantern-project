using Godot;

public partial class Enemy : Area2D
{
    [Export]
    public int Speed = 50;

    public int health = 2;
    
    public PathFollow2D follow = new PathFollow2D();

    public override void _Ready()
    {
        // Add enemy on all loops at once
        var path = GetNode<Node>("./EnemyPaths").GetChildren()[(int)(GD.Randi() % GetNode<Node>("./EnemyPaths").GetChildCount())];
        GD.Print(path.GetPath());
        path.AddChild(follow);
        follow.Loop = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        follow.Progress += Speed * (float)delta;
        Position = follow.Position;
        if (follow.ProgressRatio >= 1)
        {
            QueueFree();
        }

    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            DieEnemy();
        }
        // When have a death animation uncomment this
        //var DeathAnimation = GetNode<AnimatedSprite2D>("./AnimatedSprite2D");
        //DeathAnimation.Play("death");
        //var DeathCollision = GetNode<CollisionShape2D>("./CollisionShape2D");
        //DeathCollision.SetDeferred("disabled", true);
        //SetPhysicsProcess(false);
    }

    public void DieEnemy()
    {
        Speed = 0;
        GetNode<CollisionShape2D>("./CollisionShape2D").SetDeferred("disabled", true);
        GetNode<Sprite2D>("./Sprite2D").Hide();
        QueueFree();
    }

    public void OnAreaEntered(Node2D area)
    {
        GD.Print("Area Entered: Enemy");
        GD.Print(area.GetGroups());
        if (area.IsInGroup("attack"))
        {
            Enemy enemy = (Enemy)area;
            enemy.TakeDamage(1);
        }
    }

    //public void OnAnimatedSprite2DAnimationFinished(string AnimationName)
    //{
    //    if (AnimationName == "death")
    //    {
    //        QueueFree();
    //    }
    //}


    //public int FacingX = 1;
    //public int FacingY = 1;

    //public override void _PhysicsProcess(double delta)
    //{
    //    var velocity = Vector2.One;
    //    // Need to decide how the enemy is going to move otherwise this will not actually move the 
    //    // enemy.
    //    //velocity = velocity * Speed *(float)delta;
    //    velocity.X = velocity.X * Speed * (float)delta;
    //    velocity.Y = velocity.Y * Speed * (float)delta;

    //    Velocity = velocity;

    //    MoveAndSlide();
    //    for (int i =0; i < GetSlideCollisionCount(); i++)
    //    {
    //        KinematicCollision2D collision = GetSlideCollision(i);
    //        if (collision.GetCollider() is Player)
    //        {
    //            // The player gets hurt. Requires Player to have Hurt()
    //            //collision.GetCollider().Hurt();
    //        }
    //        if (collision.GetNormal().X != 0)
    //        {
    //            // Turn the enemy around if they run into an object in the x direction
    //            FacingX = Mathf.Sign(collision.GetNormal().X);
    //        }
    //        if (collision.GetNormal().Y != 0)
    //        {
    //            // Turn the enemy around if they run into an object in the y direction
    //            FacingY = Mathf.Sign(collision.GetNormal().Y);
    //        }

    //    }
    //}

    // Add the following to the player script to hopefully utilise this hit and death code.
    // for (int i=0; i< GetSlideCollisionCount(); i++){
    // KinematicCollision2D collision = GetSlideCollision(i);
    //     if(collision.GetCollider().IsInGroup("enemies"){
    //         collision.GetCollider().TakeDamage()
    //     }
    //     else{
    //          Hurt();
    //     }
    // }
}
