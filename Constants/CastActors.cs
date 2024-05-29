using Godot;
using System;
using System.Linq;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

/// <summary>
/// When a new Actor needs to be available for a .dialogue file to direct it, add it as a property in CastActors.
/// This will allow Dialogue Manager access to it and its' generic Action methods.
/// </summary>
public partial class CastActors : Node
{
    public ActorNode luce;
    
    public Player player;

    public override void _Ready()
    {
        var tree = GetTree();
        var playerNodes = tree.GetNodesInGroup(NodeGroup.Player);
        player = playerNodes.Cast<Player>().FirstOrDefault();
        
        var actorNodes = tree.GetNodesInGroup(NodeGroup.ActorNode);
        luce = actorNodes.Cast<ActorNode>().FirstOrDefault();
    }
}
