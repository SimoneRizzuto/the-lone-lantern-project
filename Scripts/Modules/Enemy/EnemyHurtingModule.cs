using Godot;
using System.Linq;
using TheLoneLanternProject.Scripts.Modules.Interactables;
using TheLoneLanternProject.Scripts.Shared.Constants;
//using TheLoneLanternProject.Scripts.StateMachines.Enemy;

namespace TheLoneLanternProject.Scripts.Modules.Enemy;

[GlobalClass]
public partial class EnemyHurtingModule : Node
{
    [Export] public EnemyStateMachine State;
    private Area2D collisionShape;

    public override void _Ready()
    {
        State ??= GetParent<EnemyStateMachine>();
        collisionShape = GetNode<Area2D>(GetParent().GetParent().GetPath() + "/HitBox");
        
    }
    public override void _Process(double delta)
    {
       
        //GD.Print(State.EnemyState);
        CheckIfHurt();
        //if (State.EnemyState != EnemyState.Hurting) return;
        //QueueFree();
        
        
    }

    public void CheckIfHurt()
    {
        var overlappingAreas = collisionShape.GetOverlappingAreas().ToList();
        GD.Print(overlappingAreas.Count);
        foreach (var area in overlappingAreas)
        {
            GD.Print(area);
        }
        
        if (overlappingAreas.Count > 0)
        {
            State.EnemyState = EnemyState.Hurting; // in future will need a better health and damage system then one hit then dead
        }
    }
}
