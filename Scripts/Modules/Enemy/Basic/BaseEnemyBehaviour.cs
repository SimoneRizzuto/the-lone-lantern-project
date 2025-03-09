using Godot;
using System.Diagnostics;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;
public partial class BaseEnemyBehaviour : Node
{
    protected Stopwatch Timer = new();
    protected EnemyStateMachine StateMachine => GetParent<EnemyStateMachine>();
    protected Luce Luce => GetNodeHelper.GetLuce(GetTree());
    protected AnimatedSprite2D MainSprite => GetParent().GetParent().GetNode<AnimatedSprite2D>("MainSprite");
    
    protected virtual void CheckDistanceToLuce()
    {
        var distance = StateMachine.EnemyTemplate.Position.DistanceTo(Luce.Position);
        if (distance <= EnemyConstants.InitiateCombatDistance)
        {
            StateMachine.EnemyState = EnemyState.CombatReposition;
        }
        else
        {
            StateMachine.EnemyState = EnemyState.OutOfCombatIdle;
        }
    }
}