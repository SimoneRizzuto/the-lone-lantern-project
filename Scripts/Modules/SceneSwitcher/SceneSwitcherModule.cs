using System.Linq;
using Godot;
using TheLoneLanternProject.Scripts.Helpers;
using TheLoneLanternProject.Scripts.Constants;
using TheLoneLanternProject.Scripts.Modules.Camera;

namespace TheLoneLanternProject.Scripts.Modules.SceneSwitcher;

public partial class SceneSwitcherModule : Node
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

    private void HandleSceneSwitch(DoorSpawnDTO dto)
    {
        var sceneToRemove = GetChild(0);
        RemoveChild(sceneToRemove);
        
        var uid = ResourceUid.TextToId(dto.NewSceneUid);
        var path = ResourceUid.GetIdPath(uid);
        var nextScenePackedScene = (PackedScene)ResourceLoader.Load(path);
        if (nextScenePackedScene == null)
        {
            GD.PrintErr($"{nameof(nextScenePackedScene)} cannot be found. UID: {dto.NewSceneUid} - DoorName: {dto.DoorName}");
            return;
        }
        
        var sceneToAdd = nextScenePackedScene.Instantiate();
        AddChild(sceneToAdd);
        
        var tree = GetTree();
        
        // Find door by group name "door" and the "doorName".
        var doorNodes = tree.GetNodesInGroup(NodeGroup.Door);
        var door = doorNodes.Cast<Door2DModule>().FirstOrDefault(x => x.DoorName == dto.DoorName);
        if (door == null)
        {
            GD.PrintErr($"{nameof(door)} was null. UID: {dto.NewSceneUid} - DoorName: {dto.DoorName}");
            return;
        }
        
        // Find player by group name "player".
        var luce = GetNodeHelper.GetLuce(tree);
        if (luce == null)
        {
            GD.PrintErr($"{nameof(luce)} was null. UID: {dto.NewSceneUid} - DoorName: {dto.DoorName}");
            return;
        }
        
        luce.GlobalPosition = door.GlobalPosition;
        luce.SetDirection(dto.ExitDirection);

        playerCamera2D = GetNodeHelper.GetPlayerCamera2D(tree);
        playerCamera2D.PlayerOnScreenExited();
        mainCamera2D.ToNode(playerCamera2D);
    }
}