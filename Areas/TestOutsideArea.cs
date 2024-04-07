using Godot;

public partial class TestOutsideArea : Node
{
    [Signal] public delegate void SignalSceneSwitchEventHandler();

    public void TriggerSceneSwitch(PackedScene NextScene)
    {

        EmitSignal(SignalName.SignalSceneSwitch, NextScene);

    }
    
}
