using Godot;
using System;
using System.Text.RegularExpressions;
using TheLoneLanternProject.Scripts.Utils.Signals;

public partial class SpawnerModule : Marker2D
{
    private CustomSignals customSignals;

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.Spawn += Spawn;
    }

    private void Spawn()
    {
        //This works when spawnodule attached to combat module
    }
}
