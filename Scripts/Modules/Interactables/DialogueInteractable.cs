using Godot;
using DialogueManagerRuntime;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.Utils.Signals;



public partial class DialogueInteractable : Area2D, IInteractable
{
    [Export] public Resource dialogueScript; 
    [Export] public string dialogueStartString;

    private CustomSignals customSignals = new();
    private Luce luce = new();
    public override void _Ready()
    { 
        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);
    }

    public void Interact()
    {
        luce.SetState(PlayerState.Disabled); 

        DialogueManager.ShowDialogueBalloon(dialogueScript, dialogueStartString);
        DialogueManager.DialogueEnded += SetupGameplayAfterDialogueEnded;
    }

    private void SetupGameplayAfterDialogueEnded(Resource dialogueResource)
    {
        luce.SetState(PlayerState.Idle);
        DialogueManager.DialogueEnded -= SetupGameplayAfterDialogueEnded;
    }
}