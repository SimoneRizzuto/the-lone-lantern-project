using Godot;
using System;
using TheLoneLanternProject.Scripts.Constants;

namespace TheLoneLanternProject.Modules;

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
