using Godot;
using TheLoneLanternProject.Scenes.Player;

public partial class MainCamera2D : Node2D
{
    private Camera2D camera;
    private Luce luce;

    private Node2D nodeToFollow;
    
    public override void _Ready()
    {
        var cameraNode = GetNode("Camera2D");
        if (cameraNode != null)
        {
            camera = (Camera2D)cameraNode;
            // Commented out code would fix the camera starting to smooth to the player.
            // If setting the property worked correctly. Seems like a bug. Report it to Godot issues.
            //camera.PositionSmoothingEnabled = false;
        }
        
        luce = LuceHelper.GetLuce(GetTree());
        Position = luce.GlobalPosition;
        ToNode(luce);
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
        camera.MakeCurrent();
        nodeToFollow = node;
    }
}
