using Godot;
using TheLoneLanternProject.Constants;

public partial class DoorwayArea : Area2D
{
    private bool bodyEntered = false;

    [Export] public PackedScene NewScene { get; set; }

    [Signal] public delegate void SceneSwitchEventHandler(string levelName);

    private void OnBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup("player"))
        {
            bodyEntered = true;
        }
        
    }

    private void OnBodyExited(PhysicsBody2D body)
    {
        if (body.IsInGroup("player"))
        {
            bodyEntered = false;
        }
    }

    public override void _Process(double delta)
    {
        if (bodyEntered)
        {
            if (Input.IsActionJustPressed(InputMapAction.Enter))
            {

                EmitSignal(SignalName.SceneSwitch, NewScene);
            }
        }

        // Future implementation requires that this not be predatory i.e. button hold not single press.
    }
}
