using System.Linq;
using Godot;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Scenes.Camera;
public partial class Tripod2D : Node2D
{
    private Luce luce;
    private PlayerCamera2D playerCamera2D;
    private MainCamera2D mainCamera2D;
    private VisibleOnScreenEnabler2D tripodIsOnScreen;
    
    public override void _Ready()
    {
        var tree = GetTree();
        
        var tripodIsOnScreenNode = GetNode("TripodIsOnScreen");
        if (tripodIsOnScreenNode is VisibleOnScreenEnabler2D onScreenNode)
        {
            tripodIsOnScreen = onScreenNode;
        }
        
        luce = GetNodeHelper.GetLuce(tree);
        playerCamera2D = GetNodeHelper.GetPlayerCamera2D(tree);
        mainCamera2D = GetNodeHelper.GetMainCamera2D(tree);
    }
    
    // SIGNALS
    public void OnBodyEnteredMountCameraTrigger(Node2D area)
    {
        if (area.IsInGroup(NodeGroup.Player))
        {
            playerCamera2D.FollowPlayer = false;
            GDHelper.MoveNode(playerCamera2D, tripodIsOnScreen);

            mainCamera2D.ToNode(playerCamera2D);
        }
    }
}