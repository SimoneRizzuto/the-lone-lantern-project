using Godot;
using DialogueManagerRuntime;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Helpers;
using TheLoneLanternProject.Scripts.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Interactables;

[GlobalClass]
public partial class DialogueModule : Node
{
    private CustomSignals customSignals = new();
    private Luce luce = new();
    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.Interaction += DoSomething;
        customSignals.ShowDialogueBalloon += ShowDialogueBalloon;

        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);
    }

    private void DoSomething()
    {
        GD.Print("Picked Up!");
    }

    private void ShowDialogueBalloon(string dialogue, string title)
    {
        luce.SetState(PlayerState.Disabled);

        DialogueManager.ShowDialogueBalloon(GD.Load($"res://Assets/Dialogue/{dialogue}.dialogue"), title);
        DialogueManager.DialogueEnded += SetupGameplayAfterDialogueEnded;
    }

    private void SetupGameplayAfterDialogueEnded(Resource dialogueResource)
    {
        luce.SetState(PlayerState.Idle);
        DialogueManager.DialogueEnded -= SetupGameplayAfterDialogueEnded;
    }
}