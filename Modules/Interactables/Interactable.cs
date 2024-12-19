using Godot;
using System;

namespace TheLoneLanternProject.Modules;

/*
public interface IInteractable
{
    void Interact();
}


public class DialogueInteractable : Node2D, IInteractable
{
    //[Export] public Dialogue 

    public void Interact()
    {
        Console.WriteLine("Dialogue");
    }
}

public class ItemInteractable : Node2D, IInteractable
{
    private CustomSignals customSignals;
    private bool bodyEntered = false;

    private Area2D interactionLeftArea = new();
    private Area2D interactionRightArea = new();
    private Area2D interactionUpArea = new();
    private Area2D interactionDownArea = new();

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");

        interactionLeftArea = GetNode<Area2D>("InteractionZoneLeft");
        interactionRightArea = GetNode<Area2D>("InteractionZoneRight");
        interactionUpArea = GetNode<Area2D>("InteractionZoneUp");
        interactionDownArea = GetNode<Area2D>("InteractionZoneDown");
    }


    public override void _Process(double delta)
    {

        Interact();
    }
    private void OnInteractionZoneAreaEntered(Area2D area)
    {
        if (area.IsInGroup(NodeGroup.Interact))
        {
            bodyEntered = true;
        }
    }

    private void OnInteractionZoneAreaExited(Area2D area)
    {
        if (area.IsInGroup(NodeGroup.Interact))
        {
            bodyEntered = false;
        }
    }

    public void Interact()
    {
        if (bodyEntered)
        {
            if (Input.IsActionJustPressed(InputMapAction.Interact))
            {
                customSignals.EmitSignal(nameof(CustomSignals.Interaction));
                GD.Print("Sent!");
            }
        }
    }
}

public class TriggerInteractable : Node2D, IInteractable
{
    public void Interact()
    {
        Console.WriteLine("ya mum");
    }
}


public class DoThing : Area2D
{
    private IInteractable currentlyFacingInteractable;

    public override void _Ready()
    {
        BodyEntered += PassInteractable;
    }

    public override void _Process(double delta)
    {
        // if input for interacting
        {
            // 

            DoInteractableThing();
        }
    }

    private void PassInteractable(Node2D body)
    {
        var interactable = (IInteractable)body;
        currentlyFacingInteractable = interactable;
    }

    private void DoInteractableThing()
    {
        currentlyFacingInteractable.Interact();
    }
}

public void TransformInteractionCollision(Vector2 vector)
{
    var direction = DirectionHelper.GetSnappedDirection(vector);

    interactionLeftArea.RemoveFromGroup(NodeGroup.Interact);
    interactionRightArea.RemoveFromGroup(NodeGroup.Interact);
    interactionUpArea.RemoveFromGroup(NodeGroup.Interact);
    interactionDownArea.RemoveFromGroup(NodeGroup.Interact);

    if (direction == Direction.Right)
    {
        interactionLeftArea.AddToGroup(NodeGroup.Interact);
    }
    if (direction == Direction.Left)
    {
        interactionRightArea.AddToGroup(NodeGroup.Interact);
    }
    if (direction == Direction.Up)
    {
        interactionUpArea.AddToGroup(NodeGroup.Interact);
    }
    if (direction == Direction.Down)
    {
        interactionDownArea.AddToGroup(NodeGroup.Interact);
    }
}
*/



