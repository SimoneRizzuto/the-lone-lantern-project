using System.Diagnostics;
using Godot;
using TheLoneLanternProject.Scripts.Constants;
using TheLoneLanternProject.Scripts.Helpers;
using TheLoneLanternProject.Scripts.Player;

namespace TheLoneLanternProject.Scripts.Camera;

public partial class PlayerCamera2D : Camera2D
{
    private MainCamera2D mainCamera2D;
    
    private int positionValue = 30;
    private int horizontalOffset = 6;
    private int verticalOffset = 12;

    public bool FollowPlayer = true;

    private Luce luce;
    
    private bool playerIsMoving;
    private readonly Stopwatch howLongPlayerMoving = new();
    private readonly Stopwatch howLongPlayerIdle = new();
    
    private float x;
    private float y;

    private float verticalShiftSpeed = 1f;
    private float horizontalShiftSpeed = 1f;
    
    private Direction lastDirection = Direction.Down;

    public override void _Ready()
    {
        var tree = GetTree();
        
        luce = GetNodeHelper.GetLuce(tree);
        mainCamera2D = GetNodeHelper.GetMainCamera2D(tree);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (FollowPlayer)
        {
            SetCameraToAdjust();

            if (howLongPlayerMoving.ElapsedMilliseconds > 300)
            {
                SetRelativePosition(lastDirection);
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
    
    public void SetRelativePosition(Direction direction)
    {
        int maxY = 0, maxX = 0;
        
        if (direction == Direction.Up)
        {
            maxX = 0;
            maxY = -positionValue - verticalOffset - 8;
        }
        else if (direction == Direction.Left)
        {
            maxX = -positionValue - horizontalOffset;
            maxY = -verticalOffset;
        }
        else if (direction == Direction.Right)
        {
            maxX = positionValue + horizontalOffset;
            maxY = -verticalOffset;
        }
        else if (direction == Direction.Down)
        {
            maxX = 0;
            maxY = positionValue;
        }
        else if (direction == Direction.None)
        {
            Position = Vector2.Zero;
            return;
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