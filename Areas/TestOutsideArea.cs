using Godot;

public partial class TestOutsideArea : Node
{
    [Signal] public delegate void SignalSceneSwitchEventHandler();

    public void TriggerSceneSwitch(PackedScene NextScene, string DoorName)
    {

        EmitSignal(SignalName.SignalSceneSwitch, NextScene, DoorName);

    }
    
}
