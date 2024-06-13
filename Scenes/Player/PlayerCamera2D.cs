using Godot;
using System.Diagnostics;
using System.Linq;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Scenes.Player;

public partial class PlayerCamera2D : Camera2D
{
    private MainCamera2D mainCamera2D;
    
    private int positionValue = 30;
    private int horizontalOffset = 6;
    private int verticalOffset = 12;

    public bool FollowPlayer = true;

    private Luce luce;
    
    private bool playerIsMoving;
    private Stopwatch howLongPlayerMoving = new();
    private Stopwatch howLongPlayerIdle = new();
    
    private float x;
    private float y;

    private float verticalShiftSpeed = 1f;
    private float horizontalShiftSpeed = 1f;
    
    private Direction lastDirection = Direction.Down;

    public override void _Ready()
    {
        MakeCurrent();

        var tree = GetTree();
        
        luce = LuceHelper.GetLuce(tree);
        
        var transitionCameraNode = tree.GetNodesInGroup(NodeGroup.TransitionCamera).FirstOrDefault();
        if (transitionCameraNode is MainCamera2D transitionCamera)
        {
            mainCamera2D = transitionCamera;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (FollowPlayer)
        {
            SetCameraToAdjust();

            if (howLongPlayerMoving.ElapsedMilliseconds > 300)
            {
                SetPositionOnDirection();
            }
        }
    }
    
    public void SetCameraToAdjust()
    {
        if (!playerIsMoving)
        {
            howLongPlayerIdle.Start();
            return;
        }
        
        howLongPlayerMoving.Start();
    }
    
    public void SetPositionOnDirection()
    {
        int maxY = 0, maxX = 0;
        
        if (lastDirection == Direction.Up)
        {
            maxX = 0;
            maxY = -positionValue - verticalOffset - 8;
        }
        else if (lastDirection == Direction.Left)
        {
            maxX = -positionValue - horizontalOffset;
            maxY = -verticalOffset;
        }
        else if (lastDirection == Direction.Right)
        {
            maxX = positionValue + horizontalOffset;;
            maxY = -verticalOffset;
        }
        else if (lastDirection == Direction.Down)
        {
            maxX = 0;
            maxY = positionValue;
        }
        
        if (x < maxX)
        {
            x += horizontalShiftSpeed;
        }
        if (x > maxX)
        {
            x -= horizontalShiftSpeed;
        }
        
        if (y < maxY)
        {
            y += verticalShiftSpeed;
        }
        if (y > maxY)
        {
            y -= verticalShiftSpeed;
        }
        
        Position = new Vector2(x, y);
    }

    // SIGNALS
    public void OnLuceLastWalkDirection(int direction) => lastDirection = (Direction)direction;

    public void OnPlayerIsMoving(bool isMoving) => playerIsMoving = isMoving;

    public void PlayerOnScreenExited()
    {
        FollowPlayer = true;
        GDHelper.MoveNode(this, luce);
        
        mainCamera2D.ToNode(this);
    }
}
