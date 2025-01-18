using System;
using Godot;
using TheLoneLanternProject.Scripts.Player;
using TheLoneLanternProject.Scripts.Shared.Helpers;

namespace TheLoneLanternProject.Scripts.Modules.Interactables;
public partial class ItemInteractable : Area2D, IInteractable
{
    [Export] public Resource DialogueScript;
    [Export] public string DialogueStartString;

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