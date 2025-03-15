using Godot;
using System;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.Utils.Signals;

public partial class CombatModule : Area2D
{
    private CustomSignals customSignals = new();
    private Luce luce = new();
    private bool bodyEntered = false;
    public override void _Ready()
    {
        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);

        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (bodyEntered)
        {
            
            customSignals.EmitSignal(nameof(CustomSignals.Spawn)); // No payload at this point, just testing.
            
        }

    }


    // Signal Events
    private void OnBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup(NodeGroup.Player))
        {
            bodyEntered = true;
        }
    }

    private void OnBodyExited(PhysicsBody2D body)
    {
        if (body.IsInGroup(NodeGroup.Player))
        {
            bodyEntered = false;
        }
    }



}
