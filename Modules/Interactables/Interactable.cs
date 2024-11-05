using Godot;
using System;
using TheLoneLanternProject.Constants;

namespace TheLoneLanternProject.Modules;

public partial class Interactable : Node2D
{
    private CustomSignals customSignals;
    private bool bodyEntered = false;

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
    }


    public override void _Process(double delta)
    {
        if (bodyEntered)
        {
            if (Input.IsActionJustPressed(InputMapAction.Interact))
            {
                customSignals.EmitSignal(nameof(CustomSignals.Interaction));
                GD.Print("Sent!");
            }
        }
        
    }
    private void OnInteractionZoneBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup(NodeGroup.Player) )
        {
            bodyEntered = true;
        }
    }

    private void OnInteractionZoneBodyExited(PhysicsBody2D body) 
    {
        if (body.IsInGroup(NodeGroup.Player))
        {
            bodyEntered = false;
        }
    }


}
