using Godot;

namespace TheLoneLanternProject;

public interface IEnemy
{
    void FollowTarget(double delta, PhysicsBody2D target);
    void Die();
    void TakeDamage(int damage);
}