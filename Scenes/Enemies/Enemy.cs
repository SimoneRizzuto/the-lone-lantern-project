using Godot;

public partial class Enemy : Area2D
{
    [Export]
    public int Speed = 50;
    
    public PathFollow2D follow = new PathFollow2D();
    public PathFollow2D follow2 = new PathFollow2D();
    public PathFollow2D follow3 = new PathFollow2D();
    public RigidBody2D target = new RigidBody2D();

    public override void _Ready()
    {
        // Add enemy on all loops at once
        var path = GetNode<Path2D>("./EnemyPaths/Path2D");
        path.AddChild(follow);
        follow.Loop = false;
        var path2 = GetNode<Path2D>("./EnemyPaths/Path2D2");
        path2.AddChild(follow2);
        follow2.Loop = false;
        var path3 = GetNode<Path2D>("./EnemyPaths/Path2D3");
        path3.AddChild(follow3);
        follow3.Loop = false;

    }

    public override void _PhysicsProcess(double delta)
    {
        follow.Progress += Speed * (float)delta;
        Position = follow.Position;
        if (follow.ProgressRatio >= 1)
        {
            QueueFree();
        }
        follow2.Progress += Speed * (float)delta;
        Position = follow2.Position;
        if (follow2.ProgressRatio >= 1)
        {
            QueueFree();
        }
        follow3.Progress += Speed * (float)delta;
        Position = follow3.Position;
        if (follow3.ProgressRatio >= 1)
        {
            QueueFree();
        }

    }

    public void TakeDamage()
    {
        // When have a death animation uncomment this
        var DeathAnimation = GetNode<AnimatedSprite2D>("./AnimatedSprite2D");
        DeathAnimation.Play("death");
        var DeathCollision = GetNode<CollisionShape2D>("./CollisionShape2D");
        DeathCollision.SetDeferred("disabled", true);
        SetPhysicsProcess(false);
    }

    public void OnAnimatedSprite2DAnimationFinished(string AnimationName)
    {
        if (AnimationName == "death")
        {
            QueueFree();
        }
    }


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
