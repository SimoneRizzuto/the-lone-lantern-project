using Godot;
using System;
using DialogueManagerRuntime;

public partial class TestDialogue : Node
{
    public override void _Ready()
    {
        DialogueManager.ShowDialogueBalloon(GD.Load("res://Dialogue/dialogue-test.dialogue"), "start");
    }
}
