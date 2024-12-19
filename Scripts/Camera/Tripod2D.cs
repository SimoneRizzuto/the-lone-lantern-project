using Godot;
using TheLoneLanternProject.Scripts.Constants;
using TheLoneLanternProject.Scripts.Helpers;

namespace TheLoneLanternProject.Scripts.Camera;
public partial class Tripod2D : Node2D
{
    [Export] public bool DetachOnScreenExit = true;
    [Export] public Vector2 Zoom = new(1f, 1f); // CHANGE VALUE HERE TO CHANGE ZOOM
    
    private Player.Luce luce;
    private PlayerCamera2D playerCamera2D;
    private MainCamera2D mainCamera2D;
    private CollisionShape2D mount;
    
    public override void _Ready()
    {
        var tree = GetTree();

        var mountNode = GetNode("Mount");
        if (mountNode is CollisionShape2D node)
        {
            mount = node;
        }
        
        luce = GetNodeHelper.GetLuce(tree);
        playerCamera2D = GetNodeHelper.GetPlayerCamera2D(tree);
        mainCamera2D = GetNodeHelper.GetMainCamera2D(tree);
        
        mainCamera2D.Zoom = Zoom;
    }
    
    // SIGNALS
    public void OnBodyEnteredMountCameraTrigger(Node2D area)
    {
        OnBodyEntered(area);
    }

    private void OnBodyEntered(Node2D area)
    {
        if (!DetachOnScreenExit) return;
        
        GD.Print("test");
        
        if (area.IsInGroup(NodeGroup.Player))
        {
            playerCamera2D.FollowPlayer = false;
            playerCamera2D.SetRelativePosition(Direction.None);
            GDHelper.MoveNode(playerCamera2D, mount);

            mainCamera2D.ToNode(playerCamera2D);
        }
    }
}