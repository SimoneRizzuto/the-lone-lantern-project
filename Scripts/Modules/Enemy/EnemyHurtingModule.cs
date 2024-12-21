using Godot;
using TheLoneLanternProject.Modules;
using TheLoneLanternProject.Scripts.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy;

[GlobalClass]
public partial class EnemyHurtingModule : Node
{
    [Export] public StateMachines.Enemy.EnemyStateMachine State;

    public override void _Ready()
    {
        State ??= GetParent<StateMachines.Enemy.EnemyStateMachine>();
    }
    public override void _Process(double delta)
    {
        if (State.EnemyState != EnemyState.Hurting) return;
        
        
    }

}
