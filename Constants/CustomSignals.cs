using Godot;
using TheLoneLanternProject.Scenes.SceneSwitcher;

public partial class CustomSignals : Node
{
    [Signal] public delegate void SceneSwitchEventHandler(DoorSpawnAttributes attributes);
}
