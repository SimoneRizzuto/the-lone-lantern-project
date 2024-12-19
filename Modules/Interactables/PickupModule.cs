using Godot;
using System;
using TheLoneLanternProject.Helpers;

namespace TheLoneLanternProject.Modules; // May not be needed

[GlobalClass]
public partial class PickupModule : Node
{
    private CustomSignals customSignals = new();
    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.Interaction += DoSomething;
    }

    private void DoSomething()
    {
        GD.Print("Picked Up!");
    }
}
