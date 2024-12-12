using Godot;
using System.Linq;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Scenes.SceneSwitcher;

public partial class SceneSwitcher : Node
{
    private CustomSignals customSignals = new();

    private MainCamera2D mainCamera2D;
    private PlayerCamera2D playerCamera2D;

    public override void _Ready()
    {
        var tree = GetTree();

        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.SceneSwitch += HandleSceneSwitch;

        mainCamera2D = GetNodeHelper.GetMainCamera2D(tree);
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
        var luce = GetNodeHelper.GetLuce3(tree);
        if (luce == null)
        {
            GD.PrintErr($"{nameof(luce)} was null. UID: {attributes.NewSceneUid} - DoorName: {attributes.DoorName}");
            return;
        }
        
        luce.GlobalPosition = door.GlobalPosition;
        luce.SetDirection(attributes.ExitDirection);

        playerCamera2D = GetNodeHelper.GetPlayerCamera2D(tree);
        playerCamera2D.PlayerOnScreenExited();
        mainCamera2D.ToNode(playerCamera2D);
    }
}