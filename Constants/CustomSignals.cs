using Godot;
using System;

public partial class CustomSignals : Node
{
    [Signal] public delegate void SceneSwitchEventHandler(PackedScene newScene, string doorName);
}
