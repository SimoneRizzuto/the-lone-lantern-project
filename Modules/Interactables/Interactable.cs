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
    private void OnInteractionZoneAreaEntered(Area2D area)
    {
        if (area.IsInGroup(NodeGroup.Interact) )
        {
            bodyEntered = true;
        }
    }

    private void OnInteractionZoneAreaExited(Area2D area) 
    {
        if (area.IsInGroup(NodeGroup.Interact))
        {
            bodyEntered = false;
        }
    }

}
