using System;
using System.Linq;
using Godot;
using TheLoneLanternProject.Constants;

namespace TheLoneLanternProject.Scenes.SceneSwitcher;

public partial class SceneSwitcher : Node
{
    private CustomSignals customSignals = new();

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.SceneSwitch += HandleSceneSwitch;
    }

    private void HandleSceneSwitch(DoorSpawnAttributes attributes)
    {
        var sceneToRemove = GetChild(0);
        RemoveChild(sceneToRemove);
        
        var uid = ResourceUid.TextToId(attributes.NewSceneUid);
        var path = ResourceUid.GetIdPath(uid);
        var nextScenePackedScene = (PackedScene)ResourceLoader.Load(path);
        if (nextScenePackedScene == null)
        {
            GD.PrintErr($"{nameof(nextScenePackedScene)} cannot be found. UID: {attributes.NewSceneUid} - DoorName: {attributes.DoorName}");
            return;
        }
        
        var sceneToAdd = nextScenePackedScene.Instantiate();
        AddChild(sceneToAdd);
        
        var tree = GetTree();
        
        // Find door by group name "door" and the "doorName".
        var doorNodes = tree.GetNodesInGroup(NodeGroup.Door);
        var door = doorNodes.Cast<Door2D>().FirstOrDefault(x => x.DoorName == attributes.DoorName);
        if (door == null)
        {
            GD.PrintErr($"{nameof(door)} was null. UID: {attributes.NewSceneUid} - DoorName: {attributes.DoorName}");
            return;
        }
        
        // Find player by group name "player".
        var playerNode = tree.GetNodesInGroup(NodeGroup.Player).FirstOrDefault();
        if (playerNode == null)
        {
            GD.PrintErr($"{nameof(playerNode)} was null. UID: {attributes.NewSceneUid} - DoorName: {attributes.DoorName}");
            return;
        }
        
        var player = (Player.Player)playerNode;
        player.GlobalPosition = door.GlobalPosition;
        player.Direction = attributes.ExitDirection;
    }
}