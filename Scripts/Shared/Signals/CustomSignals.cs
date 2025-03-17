using Godot;
using TheLoneLanternProject.Scripts.Modules.SceneSwitcher;

namespace TheLoneLanternProject.Scripts.Utils.Signals;
public partial class CustomSignals : Node
{
    [Signal] public delegate void SceneSwitchEventHandler(DoorSpawnDTO dto);
    
    [Signal] public delegate void ShowDialogueBalloonEventHandler(string dialogue, string title);

    [Signal] public delegate void InteractionEventHandler(); // This will probably need to have a payload that gives some information to the modules

    [Signal] public delegate void SpawnEventHandler(); //PackedScene scene, string id
}