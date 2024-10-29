using Godot;
using TheLoneLanternProject.Scenes.SceneSwitcher;

public partial class CustomSignals : Node
{
    [Signal] public delegate void SceneSwitchEventHandler(DoorSpawnAttributes attributes);
    
    [Signal] public delegate void ShowDialogueBalloonEventHandler(string dialogue, string title);

    [Signal] public delegate void InteractionEventHandler(); // This will probably need to have a payload that gives some information to the modules
}
