using System.Linq;
using Godot;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Scenes.Camera;
public partial class Tripod2D : Node2D
{
    private PlayerCamera2D playerCamera2D;
    private MainCamera2D mainCamera2D;
    private VisibleOnScreenEnabler2D tripodIsOnScreen;
    
    private Luce luce;
    
    public override void _Ready()
    {
        var tree = GetTree();
        
        luce = LuceHelper.GetLuce(tree);
        
        var playerCameraNode = tree.GetNodesInGroup(NodeGroup.PlayerCamera).FirstOrDefault();
        if (playerCameraNode is PlayerCamera2D playerCamera)
        {
            playerCamera2D = playerCamera;
        }
        
        var transitionCameraNode = tree.GetNodesInGroup(NodeGroup.MainCamera).FirstOrDefault();
        if (transitionCameraNode is MainCamera2D transitionCamera)
        {
            mainCamera2D = transitionCamera;
        }

        var tripodIsOnScreenNode = GetNode("TripodIsOnScreen");
        if (tripodIsOnScreenNode is VisibleOnScreenEnabler2D onScreenNode)
        {
            tripodIsOnScreen = onScreenNode;
        }
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