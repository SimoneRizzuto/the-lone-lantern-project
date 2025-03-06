using Godot;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;
public partial class BaseBasicEnemyBehaviour : Node
{
    public EnemyStateMachine StateMachine => GetParent<EnemyStateMachine>();
    public Luce Luce => GetNodeHelper.GetLuce(GetTree());
}