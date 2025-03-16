using Godot;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.StateMachines.Enemy;
using TheLoneLanternProject.Scripts.Modules.Enemy.Attacks;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;
public partial class BaseEnemyBehaviour : Node
{
    protected Stopwatch Timer = new();
    protected EnemyStateMachine StateMachine => GetParent<EnemyStateMachine>();
    protected Luce Luce => GetNodeHelper.GetLuce(GetTree());
    protected AnimatedSprite2D MainSprite => GetParent().GetParent().GetNode<AnimatedSprite2D>("MainSprite");
    protected List<BaseEnemyAttack> Attacks => GetChildren().OfType<BaseEnemyAttack>().ToList();
    
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