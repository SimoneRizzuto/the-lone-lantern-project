using Godot;
using System.Diagnostics;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;
public partial class BaseEnemyBehaviour : Node
{
    public EnemyStateMachine StateMachine => GetParent<EnemyStateMachine>();
    public Luce Luce => GetNodeHelper.GetLuce(GetTree());
    public Stopwatch Timer = new();
}