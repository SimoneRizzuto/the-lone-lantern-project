using System.Linq;
using Godot;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;

namespace TheLoneLanternProject.Scripts.Modules.Interactables;

[GlobalClass]
public partial class PlayerInteractorModule : Node
{
    private Area2D interactableDetector;
    private Marker2D directionMarker = new();
    
    private Luce luce = new();

    public override void _Ready()
    {
        directionMarker = GetNode<Marker2D>(GetParent().GetParent().GetPath() + "/InteractorDirection");
        interactableDetector = GetNode<Area2D>(GetParent().GetParent().GetPath() + "/InteractorDirection/InteractableDetector");

        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);
    }


    public override void _Process(double delta)
    {
        ChangeDirection();
        CheckInteractionButton();
    }

    private void ChangeDirection()
    {
        var stateMachine = luce.GetStateMachine();
        if (stateMachine.LastDirection == Direction.Left)
        {
            directionMarker.Rotation = Mathf.DegToRad(90);
        }
        else if (stateMachine.LastDirection == Direction.Right)
        {
            directionMarker.Rotation = Mathf.DegToRad(270);
        }
        else if (stateMachine.LastDirection == Direction.Up)
        {
            directionMarker.Rotation = Mathf.DegToRad(180);
        }
        else if (stateMachine.LastDirection == Direction.Down)
        {
            directionMarker.Rotation = 0;
        }
    }
    private void CheckInteractionButton()
    {
        if (Input.IsActionJustPressed(InputMapAction.Interact))
        {
            var overlappingAreas = interactableDetector.GetOverlappingAreas().ToList();
            var interactables = overlappingAreas.FindAll(x => x is IInteractable);

            if (interactables.Count() < overlappingAreas.Count())
            {
                GD.PrintErr("Detected non IInteractable while processing interaction. Please don't put Area2D's that aren't IInteractables to Collision Layer 4.");
            }

            if (interactables.Count > 0)
            {
                var firstInteractable = (IInteractable)interactables[0];
                
                luce.SetPlayerState(PlayerState.Disabled);
                
                firstInteractable.Interact();
            }
        }
    }
}