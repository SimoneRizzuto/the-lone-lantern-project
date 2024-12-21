using Godot;
using TheLoneLanternProject.Scripts.Constants;
using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy;

[GlobalClass]
public partial class EnemyHurtingModule : Node
{
    [Export] public EnemyStateMachine State;

    public override void _Ready()
    {
        State ??= GetParent<EnemyStateMachine>();
    }
    public override void _Process(double delta)
    {
        if (State.EnemyState != EnemyState.Hurting) return;
        
        
    }

}
