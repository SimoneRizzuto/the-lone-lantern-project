using System;
using Godot;
using System.Linq;
using TheLoneLanternProject.Scripts.Shared.Constants;

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

        collisionShape.BodyShapeEntered += MethodWoo;
        collisionShape.BodyEntered += MethodWoo2;
    }

    private void MethodWoo2(Node2D body)
    {
        if (body.Name == "Hitbox")
        {
            Console.WriteLine("Hi :)");
        }
    }
    
    private void MethodWoo(Rid bodyrid, Node2D body, long bodyshapeindex, long localshapeindex)
    {
        if (body.Name == "Hitbox")
        {
            Console.WriteLine("Hi :)");
        }
    }

    public override void _Process(double delta)
    {
        if (State.EnemyState != EnemyState.Hurting) return;
        
        CheckIfHurt();
        //QueueFree();
    }

    public void CheckIfHurt()
    {
        var overlappingAreas = collisionShape.GetOverlappingAreas().ToList();
        //GD.Print(overlappingAreas.Count);
        foreach (var area in overlappingAreas)
        {
            //GD.Print(area);
        }
        
        if (overlappingAreas.Count > 0)
        {
            State.EnemyState = EnemyState.Hurting; // in future will need a better health and damage system then one hit then dead
        }
    }
}
