using Godot;
using System;
using System.Linq;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

public partial class Tripod2D : Node2D
{
    private PlayerCamera2D playerCamera2D;
    private Luce luce;

    private bool followPlayer = true;
    
    public override void _Ready()
    {
        var tree = GetTree();
        
        var luceNode = tree.GetNodesInGroup(NodeGroup.Player).FirstOrDefault();
        if (luceNode is Luce player)
        {
            luce = player;
        }
        
        var camera2DNode = tree.GetNodesInGroup(NodeGroup.PlayerCamera).FirstOrDefault();
        if (camera2DNode is PlayerCamera2D camera)
        {
            playerCamera2D = camera;
        }
        
        GD.Print("camera fetched");
    }

    
    private void MoveNode(Node child, Node parent)
    {
        // Get the current parent of the node
        var currentParent = child.GetParent();
        if (currentParent != null)
        {
            // Remove the node from its current parent
            currentParent.RemoveChild(child);
        }
        
        // Add the node to the new parent
        parent.AddChild(child);
        child.Owner = parent;
    }
    
    // SIGNALS
    public void OnBodyEnteredMountCameraTrigger(Node2D area)
    {
        if (area.IsInGroup(NodeGroup.Player))
        {
            playerCamera2D.FollowPlayer = false;
            MoveNode(playerCamera2D, this);
        }
    }
    
    
    /*public void OnScreenEntered()
    {
        // CHANGE THIS, instead, trigger this with an area 2D
        
        playerCamera2D.FollowPlayer = false;
        
        // instead, slowly transition to position, THEN LOCK NODE
        
        
        
        MoveNode(playerCamera2D, this);
    }*/
}
