using Godot;
using System;
using System.Linq;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

/// <summary>
/// Anytime a new Actor needs to be available for a .dialogue file to be give directions to, it needs to be added as a property in here.
/// This is so the Dialogue Manager library has access to it so the method calls can be done.
/// </summary>
public partial class CastActors : Node
{
    public Player player;

    public override void _Ready()
    {
        var tree = GetTree();
        var playerNodes = tree.GetNodesInGroup(NodeGroup.Player);
        player = playerNodes.Cast<Player>().FirstOrDefault();
    }
}
