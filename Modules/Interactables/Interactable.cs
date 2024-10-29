using Godot;
using System;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Helpers;

namespace TheLoneLanternProject.Modules;

public partial class Interactable : Node
{
    private CustomSignals customSignals = new();

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
    }
    private void OnInteractionZoneBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup(NodeGroup.Player) && Input.IsActionJustPressed(InputMapAction.Interact))
        {
            customSignals.EmitSignal(nameof(CustomSignals.InteractionEventHandler));
        }
    }
}
