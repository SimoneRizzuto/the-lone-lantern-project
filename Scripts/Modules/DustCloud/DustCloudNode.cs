using Godot;

namespace TheLoneLanternProject.Scripts.Modules.DustCloud;
public partial class DustCloudNode : Node2D
{
    private AnimatedSprite2D animatedSprite2D;
    
    public virtual void Play_DashCloud(Vector2? position = null)
    {
        if (animatedSprite2D == null)
        {
            animatedSprite2D ??= GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        }
        
        animatedSprite2D.AnimationFinished += Despawn;
        animatedSprite2D.Play("dash cloud");
    }
    
    private void Despawn()
    {
        QueueFree();
    }
}