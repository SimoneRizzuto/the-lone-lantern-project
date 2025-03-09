using Godot;
using System.Diagnostics;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;
public partial class BaseEnemyBehaviour : Node
{
    protected Stopwatch Timer = new();
    protected EnemyStateMachine StateMachine => GetParent<EnemyStateMachine>();
    protected Luce Luce => GetNodeHelper.GetLuce(GetTree());
    protected AnimatedSprite2D MainSprite => Owner.Owner.GetNode<AnimatedSprite2D>("MainSprite");
}