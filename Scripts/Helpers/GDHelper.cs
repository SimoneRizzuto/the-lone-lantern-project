using Godot;

namespace TheLoneLanternProject.Scripts.Helpers;

public static class GDHelper
{
    public static void MoveNode(Node child, Node parent)
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
}