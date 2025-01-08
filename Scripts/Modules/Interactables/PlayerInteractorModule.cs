using Godot;
using System;
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
        directionMarker = GetNode<Marker2D>("Marker2D");
        interactableDetector = GetNode<Area2D>("Marker2D/InteractableDetector");
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
            directionMarker.Rotation = 90;
        }
        else if (direction == Direction.Right)
        {
            directionMarker.Rotation = 270;
        }
        else if (direction == Direction.Up)
        {
            directionMarker.Rotation = 180;
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
            var interactable = interactableDetector.GetOverlappingAreas();

            if (interactable.Count > 0)
            {
                var topInteractable = interactable[0];
                GD.Print(topInteractable);
                /*var classTopInteractable = topInteractable;
                topInteractable = (classTopInteractable)topInteractable;
                topInteractable.Interact();
                return;*/
            }


        }
    }
}
