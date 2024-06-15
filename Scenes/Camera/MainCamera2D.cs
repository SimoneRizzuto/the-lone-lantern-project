using System.Linq;
using Godot;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

public partial class MainCamera2D : Node2D
{
    private Camera2D mainCamera2D;
    private Camera2D playerCamera2D;

    private Node2D nodeToFollow;
    
    public override void _Ready()
    {
        var cameraNode = GetNode("Camera2D");
        if (cameraNode != null)
        {
            mainCamera2D = (Camera2D)cameraNode;
            // Commented out code would fix the camera starting to smooth to the player.
            // If setting the property worked correctly. Seems like a bug. Report it to Godot issues.
            //camera.PositionSmoothingEnabled = false;
        }
        
        var playerCamera2DNodes = GetTree().GetNodesInGroup(NodeGroup.PlayerCamera);
        playerCamera2D = playerCamera2DNodes.Cast<Camera2D>().FirstOrDefault();
        if (playerCamera2D == null)
        {
            GD.PrintErr($"{nameof(playerCamera2D)} was null.");
            return;
        }
        
        Position = playerCamera2D.GlobalPosition;
        ToNode(playerCamera2D);
    }

    public override void _Process(double delta)
    {
        if (nodeToFollow != null)
        {
            // Commented out code would work if setting the property worked correctly. Seems like a bug. Report it to Godot issues.
            //camera.PositionSmoothingEnabled = true;
            //camera.PositionSmoothingSpeed = 0.5f;
            Position = nodeToFollow.GlobalPosition;
        }
    }
    
    public void ToNode(Node2D node)
    {
        mainCamera2D.MakeCurrent();
        nodeToFollow = node;
    }
}
