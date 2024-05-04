using Godot;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.SceneSwitcher;

public partial class Door2D : Area2D
{
    [Export] public string SceneUID;
    [Export] public string DoorName;
    [Export] public Direction ExitDirection = Direction.Down;

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
        if (bodyEntered && Input.IsActionJustPressed(InputMapAction.Enter))
        {
            var doorSpawnAttribute = new DoorSpawnAttributes
            {
                NewSceneUid = SceneUID,
                DoorName = DoorName,
                ExitDirection = ExitDirection
            };
            
            customSignals.EmitSignal(nameof(CustomSignals.SceneSwitch), doorSpawnAttribute);
        }
    }

    // Signal Events
    private void OnBodyEntered(PhysicsBody2D body)
    {
        if (body.IsInGroup(NodeGroup.Player))
        {
            bodyEntered = true;
        }
    }

    private void OnBodyExited(PhysicsBody2D body)
    {
        if (body.IsInGroup(NodeGroup.Player))
        {
            bodyEntered = false;
        }
    }
}
