using Godot;
using TheLoneLanternProject.Constants;

namespace TheLoneLanternProject.Scenes.SceneSwitcher;
public partial class DoorSpawnAttributes : GodotObject
{
    public string NewSceneUid { get; set; }
    public string DoorName { get; set; }
    public Direction ExitDirection { get; set; }
}