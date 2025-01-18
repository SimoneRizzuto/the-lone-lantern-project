using Godot;
using System;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using TheLoneLanternProject.Scripts.Utils.Signals;

public partial class ItemInteractable : Area2D, IInteractable
{

    [Export] public Resource dialogueScript;
    [Export] public string dialogueStartString;

    private Luce luce = new();

    public override void _Ready()
    {
        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);
    }

    public void Interact()
    {
        throw new NotImplementedException();
    }
}
