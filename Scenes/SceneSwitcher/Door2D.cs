using Godot;
using TheLoneLanternProject.Constants;

namespace TheLoneLanternProject.Scenes.SceneSwitcher;
public partial class Door2D : Area2D
{
    [Export] public string SceneUID;
    [Export] public string DoorName;
    [Export] public Direction ExitDirection = Direction.Down;
    [Export] public bool TriggerByInteract = true;

    private CustomSignals customSignals;
    private bool bodyEntered = false;

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
    }

    public override void _Process(double delta)
    {
        if (bodyEntered)
        {
            if (TriggerByInteract && Input.IsActionJustPressed(InputMapAction.Enter))
            {
                TriggerTransition();
            }
            else if (!TriggerByInteract)
            {
                TriggerTransition();
            }
        }
    }
    private void TriggerTransition()
    {
        var doorSpawnAttribute = new DoorSpawnAttributes
        {
            NewSceneUid = SceneUID,
            DoorName = DoorName,
            ExitDirection = ExitDirection
        };
        
        customSignals.EmitSignal(nameof(CustomSignals.SceneSwitch), doorSpawnAttribute);
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
