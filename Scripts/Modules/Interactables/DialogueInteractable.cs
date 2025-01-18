using DialogueManagerRuntime;
using Godot;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.Utils.Signals;

namespace TheLoneLanternProject.Scripts.Modules.Interactables;
public partial class DialogueInteractable : Area2D, IInteractable
{
    [Export] public Resource DialogueScript;
    [Export] public string DialogueStartString;
    
    private bool isInteracting;
    
    private CustomSignals customSignals = new();
    private Luce luce = new();
    
    public override void _Ready()
    { 
        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);
    }

    public void Interact()
    {
        if (luce.GetStateMachine().IsInteracting) return;
        
        DialogueManager.ShowDialogueBalloon(DialogueScript, DialogueStartString);
        DialogueManager.DialogueEnded += SetupGameplayAfterDialogueEnded;

        luce.ToggleInteracting(true);
    }

    private void SetupGameplayAfterDialogueEnded(Resource dialogueResource)
    {
        luce.SetPlayerState(PlayerState.Idle);
        luce.ToggleInteracting(false);
        
        DialogueManager.DialogueEnded -= SetupGameplayAfterDialogueEnded;
    }
}