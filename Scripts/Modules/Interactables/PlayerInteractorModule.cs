using Godot;
using System;
using System.Linq;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.StateMachines.Player;

[GlobalClass]
public partial class PlayerInteractorModule: Node
{
    public Area2D interactableDetector;
    public Marker2D directionMarker = new();
    public PlayerStateMachine State;
    public Luce luce = new();
    private Vector2 directionVector;


    public override void _Ready()
    {
        directionMarker = GetNode<Marker2D>(GetParent().GetParent().GetPath() + "/InteractorDirection");
        interactableDetector = GetNode<Area2D>(GetParent().GetParent().GetPath() + "/InteractorDirection/InteractableDetector");
        State ??= GetParent<PlayerStateMachine>();

        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);
    }


    public override void _Process(double delta)
    {
        changeDirection();
        checkInteractionButton();
    }

    private void changeDirection()
    {
        directionVector = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);
        var direction = DirectionHelper.GetSnappedDirection(directionVector);
        

        if (direction == Direction.Left)
        {
            directionMarker.Rotation = Mathf.DegToRad(90);
        }
        else if (direction == Direction.Right)
        {
            directionMarker.Rotation = Mathf.DegToRad(270);
        }
        else if (direction == Direction.Up)
        {
            directionMarker.Rotation = Mathf.DegToRad(180);
        }
        else if (direction == Direction.Down)
        {
            directionMarker.Rotation = 0;
        }
        else
        {
            return;
        }
    }
    private void checkInteractionButton()
    {
        if (Input.IsActionJustPressed(InputMapAction.Interact)) {
            var interactableOverlappingAreas = interactableDetector.GetOverlappingAreas().ToList();
            var interactablesVerified = interactableOverlappingAreas.FindAll(x => x is IInteractable);

            if (interactablesVerified.Count() < interactableOverlappingAreas.Count()) {
                GD.PrintErr("Detected non IInteractable while processing interaction. Please don't put Area2D's that aren't IInteractables to Collision Layer 4.");
            }

            if (interactablesVerified.Count > 0)
            {
                var topInteractableDialogue = (IInteractable)interactablesVerified[0];
                topInteractableDialogue.Interact();

            }


        }
    }


}
