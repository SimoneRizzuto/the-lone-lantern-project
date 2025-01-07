using Godot;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Utils.Signals;

public partial class ItemInteractable : Area2D, IInteractable
{
    private CustomSignals customSignals;
    private bool bodyEntered = false;

    /*private Area2D interactionLeftArea = new();
     private Area2D interactionRightArea = new();
     private Area2D interactionUpArea = new();
     private Area2D interactionDownArea = new();*/

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");

        //AreaEntered += OnInteractionZoneAreaEntered;
        //AreaExited += OnInteractionZoneAreaExited;

        /*interactionLeftArea = GetNode<Area2D>("InteractionZoneLeft");
        interactionRightArea = GetNode<Area2D>("InteractionZoneRight");
        interactionUpArea = GetNode<Area2D>("InteractionZoneUp");
        interactionDownArea = GetNode<Area2D>("InteractionZoneDown");*/
    }


    public override void _Process(double delta)
    {

        Interact();
    }
    /*private void OnInteractionZoneAreaEntered(Area2D area)
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
    }*/

    public void Interact()
    {
        if (bodyEntered)
        {
            if (Input.IsActionJustPressed(InputMapAction.Interact))
            {
                customSignals.EmitSignal(nameof(CustomSignals.Interaction)); //this signal was sent to a function that printed some string
                // instead maybe work on this for dialogue example for trailer and is this a good structure? It is generic but it would need some way of identifying which dialogue / item.\
                // Maybe do it like the scene switcher where we have the uid of the parent that this then uses to look up in a dictionary and then use that to 
                GD.Print("Sent!");
            }
        }
    }
}
