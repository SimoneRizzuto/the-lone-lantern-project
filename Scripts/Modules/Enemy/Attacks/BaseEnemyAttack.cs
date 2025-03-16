using Godot;
using System.Diagnostics;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Attacks;
public partial class BaseEnemyAttack : Node
{
    // General
    [Export] public string AttackName = "";
    [Export] public float AttackSpeed = -1;
    [Export] public float AttackLength = -1;
    
    // Animation
    [Export] public string AttackAnimationName = "";
    [Export] public string AttackWindupAnimation = "";
    
    [Export] public int AttackAnimationHitBoxBeginFrame = -1;
    [Export] public int AttackAnimationHitBoxFinishFrame = -1;
    
    protected bool Triggered;
    protected bool WindingUp;
    
    public virtual void TriggerAttack()
    {
        Triggered = true;
    }
    
    protected Stopwatch Timer = new();
    protected EnemyStateMachine StateMachine => GetParent().GetParent<EnemyStateMachine>();
    protected Luce Luce => GetNodeHelper.GetLuce(GetTree());
    protected AnimatedSprite2D MainSprite => GetParent().GetParent().GetParent().GetNode<AnimatedSprite2D>("MainSprite");
    protected Vector2 DirectionToPlayer => StateMachine.EnemyTemplate.Position.DirectionTo(Luce.Position);
    protected Area2D Hitbox => GetChild<Area2D>(0);
}