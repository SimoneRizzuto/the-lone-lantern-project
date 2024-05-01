using Godot;
using System;

public partial class CustomSignals : Node
{
    [Signal] public delegate void SceneSwitchEventHandler(string newScene, string doorName);
}
