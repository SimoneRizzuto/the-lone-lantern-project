using Godot;
using DialogueManagerRuntime;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.Utils.Signals;



public partial class DialogueInteractableModule : Area2D, IInteractable
{
    [Export] public DialogueManager dialogueScript; // fix this 

    private CustomSignals customSignals = new();
    private Luce luce = new();
    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.Interaction += DoSomething;
        /*customSignals.ShowDialogueBalloon += Interact;

        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);*/
    }

    private void DoSomething()
    {
        GD.Print("Picked Up!");
    }

    public void Interact()
    {
        /*luce.SetState(PlayerState.Disabled);

        DialogueManager.ShowDialogueBalloon(GD.Load($"res://Assets/Dialogue/{dialogue}.dialogue"), title);
        DialogueManager.DialogueEnded += SetupGameplayAfterDialogueEnded;*/
    }

    /*private void SetupGameplayAfterDialogueEnded(Resource dialogueResource)
    {
        luce.SetState(PlayerState.Idle);
        DialogueManager.DialogueEnded -= SetupGameplayAfterDialogueEnded;
    }*/
}