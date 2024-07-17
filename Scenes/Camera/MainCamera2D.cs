using Godot;
using System.Linq;
using TheLoneLanternProject.Constants;

public partial class MainCamera2D : Node2D
{
    [Export] public Vector2 Zoom = new(2, 2);
    
    private Camera2D mainCamera2D;
    private Camera2D playerCamera2D;

    private Node2D nodeToFollow;
    
    public override void _Ready()
    {
        var cameraNode = GetNode("Camera2D");
        if (cameraNode != null)
        {
            mainCamera2D = (Camera2D)cameraNode;
            mainCamera2D.PositionSmoothingEnabled = false;
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
            mainCamera2D.PositionSmoothingEnabled = true;
            Position = nodeToFollow.GlobalPosition;
        }
        
        mainCamera2D.Zoom = Zoom;
    }
    
    public void ToNode(Node2D node)
    {
        mainCamera2D.PositionSmoothingEnabled = false;
        mainCamera2D.MakeCurrent();
        nodeToFollow = node;
    }
}
