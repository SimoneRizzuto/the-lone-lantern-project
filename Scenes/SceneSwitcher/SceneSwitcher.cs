using System;
using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;
using TheLoneLanternProject.Constants;

public partial class SceneSwitcher : Node
{
    private CustomSignals customSignals = new();
    
    private Dictionary doorDictionary = new()
    {
        {"RightDoor", "StartPositionLeft"},
        {"LeftDoor", "StartPositionRight"},
        {"HouseDoor", "StartPosition"}
    };

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.SceneSwitch += HandleSceneSwitch;
    }

    private void HandleSceneSwitch(PackedScene newScene, string doorName)
    {
        // Get the starting position based on the doorname
        var marker2DName = doorDictionary[doorName];
        if (marker2DName.AsStringName() == "")
        {
            GD.PrintErr("DoorName from DoorwayArea does not match any corresponding key in doorDictionary.");
            return;
        }
        
        var currentSceneName = GetChild(0);
        var nextSceneName = newScene.Instantiate();
        RemoveChild(currentSceneName);
        AddChild(nextSceneName);
        
        var player = GetNode<Node2D>("./" + nextSceneName.Name + "/GameContainer/PlayerController");
        try
        {
            var allDoors = GetTree().GetNodesInGroup(NodeGroup.Door);
            
            var newStartPosition = GetNode<Node2D>("./" + nextSceneName.Name + "/GameContainer/" + marker2DName);
            player.Position = newStartPosition.Position;
        }
        catch (Exception ex)
        {
            // ignored
        }
    }
}
