using Godot;
using System;
using System.Linq;
using DialogueManagerRuntime;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

public partial class CutsceneDirector : Node
{
    private CustomSignals customSignals = new();
    private Player player = new();
    
    /*
     * There are two options for implementing Cutscene directions:
     * 1. Use AnimationPlayer to build out custom animations. This is valuable for more complex animations.
     * 2. Implement generic commands for nodes that can be easily used using signals inside of a .dialogue file.
     *
     * The second one may be more complex, as I will need to figure out a clean way to point to a node that required a command.
     * I found this tutorial: https://www.youtube.com/watch?v=U3Mimia-904&ab_channel=NathanHoad
     * Using this method of giving an actor a command, I would want to make a generic version
     * of this so that I can attach it to every node that requires.
     * e.g. Make an Actor class, attach it to each node that needs to be an Actor. If any Actors have more complex actions, extend it.
     * 
     * 
     */
    
    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.ShowDialogueBalloon += ShowDialogueBalloon;
        
        var tree = GetTree();
        var playerNodes = tree.GetNodesInGroup(NodeGroup.Player);
        player = playerNodes.Cast<Player>().FirstOrDefault();
    }

    private void ShowDialogueBalloon(string dialogue, string title)
    {
        player.State = PlayerState.Disabled;
        
        DialogueManager.ShowDialogueBalloon(GD.Load($"res://Dialogue/{dialogue}.dialogue"), title);
        
        
    }
}
