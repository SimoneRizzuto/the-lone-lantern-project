using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;
using TheLoneLanternProject.Constants;

public partial class SceneSwitcher : Node
{
    private CustomSignals customSignals = new();

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.SceneSwitch += HandleSceneSwitch;
    }

    private void HandleSceneSwitch(string newSceneUid, string doorName)
    {
        var sceneToRemove = GetChild(0);
        RemoveChild(sceneToRemove);
        
        var uid = ResourceUid.TextToId(newSceneUid);
        var path = ResourceUid.GetIdPath(uid);
        var nextScenePackedScene = (PackedScene)ResourceLoader.Load(path);
        var sceneToAdd = nextScenePackedScene.Instantiate();
        AddChild(sceneToAdd);
        
        // Find door by group name "door" and the "doorName".
        var doorNodesInScene = GetTree().GetNodesInGroup(NodeGroup.Door);
        var door = doorNodesInScene.Cast<Door2D>().FirstOrDefault(x => x.DoorName == doorName);
        if (door == null)
        {
            GD.PrintErr($"Door cannot be found. UID: {newSceneUid} - DoorName: {doorName}");
            return;
        }
        
        var player = GetNode<Node2D>("./" + sceneToAdd.Name + "/GameContainer/PlayerController");
        player.Position = door.Position;
    }
}
