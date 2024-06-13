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
        }

        luce = LuceHelper.GetLuce(GetTree());
        ToNode(luce, false);
    }

    public override void _Process(double delta)
    {
        if (nodeToFollow != null)
        {
            Position = nodeToFollow.GlobalPosition;
        }
    }
    
    public void ToNode(Node2D node, bool? makeCurrent = true)
    {
        if (makeCurrent == true)
        {
            camera.MakeCurrent();
        }
        
        nodeToFollow = node;
    }
}
