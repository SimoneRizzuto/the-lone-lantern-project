using DialogueManagerRuntime;
using Godot;
using System;

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
        luce.State = PlayerState.Disabled;

        DialogueManager.ShowDialogueBalloon(GD.Load($"res://Dialogue/{dialogue}.dialogue"), title);
        DialogueManager.DialogueEnded += SetupGameplayAfterDialogueEnded;
    }

    private void SetupGameplayAfterDialogueEnded(Resource dialogueResource)
    {
        luce.State = PlayerState.Idle;
        DialogueManager.DialogueEnded -= SetupGameplayAfterDialogueEnded;
    }
}
