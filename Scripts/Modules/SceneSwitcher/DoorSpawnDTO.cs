using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.SceneSwitcher;
public partial class DoorSpawnDTO : GodotObject
{
    public string NewSceneUid { get; set; }
    public string DoorName { get; set; }
    public Direction ExitDirection { get; set; }
}