using Godot;
using System;
using DialogueManagerRuntime;

public partial class TestDialogue : Node
{
    public override void _Ready()
    {
        var balloonPackedScene = (PackedScene)ResourceLoader.Load("res://DialogueBalloon/balloon.tscn");
        if (balloonPackedScene == null)
        {
            GD.PrintErr($"{nameof(balloonPackedScene)} cannot be found.");
            return;
        }
        
        var sceneToAdd = balloonPackedScene.Instantiate();
        AddChild(sceneToAdd);
    }
}
