using Godot;
using System;
using DialogueManagerRuntime;

public partial class CutsceneDirector : Node
{
    private CustomSignals customSignals = new();
    
    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.ShowDialogueBalloon += ShowDialogueBalloon;
    }

    private void ShowDialogueBalloon(string dialogue, string title)
    {
        DialogueManager.ShowDialogueBalloon(GD.Load($"res://Dialogue/{dialogue}.dialogue"), title);
    }
}
