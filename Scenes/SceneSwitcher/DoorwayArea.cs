using Godot;
using TheLoneLanternProject.Constants;

public partial class DoorwayArea : Area2D
{
    [Export] public PackedScene NewScene { get; set; }

    [Export] public string DoorName;

    private CustomSignals customSignals;
    private bool bodyEntered = false;

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        
    }

    public override void _Process(double delta)
    {
        TriggerTransition();
    }

    

    private void TriggerTransition()
    {
        if (bodyEntered)
        {
            if (Input.IsActionJustPressed(InputMapAction.Enter))
            {
                customSignals.EmitSignal(nameof(CustomSignals.SceneSwitch), NewScene, DoorName);
            }
        }
    }

    // Signal Events
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
}
