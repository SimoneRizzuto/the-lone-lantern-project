using Godot;
using TheLoneLanternProject.Constants;

public partial class DoorwayArea : Area2D
{
    private bool bodyEntered = false;

    [Export] private string levelName = "level";

    [Signal] public delegate void SceneSwitchEventHandler(string levelName);

    private void OnBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup("player"))
        {
            bodyEntered = true;
            GD.Print(levelName + "entered");
        }
        
    }

    private void OnBodyExited(PhysicsBody2D body)
    {
        if (body.IsInGroup("player"))
        {
            bodyEntered = false;
            GD.Print(levelName + "exited");
        }
    }

    public override void _Process(double delta)
    {
        if (bodyEntered)
        {
            if (Input.IsActionJustPressed(InputMapAction.Enter))
            {
                GD.Print("Emit Signal");
                EmitSignal(SignalName.SceneSwitch, levelName);
            }
        }

        // Future implementation requires that this not be predatory i.e. button hold not single press.
    }
}
