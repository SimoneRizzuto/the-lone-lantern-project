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
        if (nextScenePackedScene == null)
        {
            GD.PrintErr($"Scene cannot be found. UID: {newSceneUid} - DoorName: {doorName}");
            return;
        }
        
        var sceneToAdd = nextScenePackedScene.Instantiate();
        AddChild(sceneToAdd);
        
        var tree = GetTree();
        
        // Find door by group name "door" and the "doorName".
        var doorNodes = tree.GetNodesInGroup(NodeGroup.Door);
        var door = doorNodes.Cast<Door2D>().FirstOrDefault(x => x.DoorName == doorName);
        if (door == null)
        {
            GD.PrintErr($"Door cannot be found. UID: {newSceneUid} - DoorName: {doorName}");
            return;
        }

        // Find player by group name "player".
        var playerNode = tree.GetNodesInGroup(NodeGroup.Player).FirstOrDefault();
        if (playerNode == null)
        {
            GD.PrintErr($"Player cannot be found. UID: {newSceneUid} - DoorName: {doorName}");
            return;
        }

        try
        {
            var playerNode2D = (Node2D)playerNode;
            playerNode2D.Position = door.GlobalPosition;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Placing the player on a door failed: {ex.Message}");
        }
    }
}
