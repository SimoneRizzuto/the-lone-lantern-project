using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

public partial class PlayerCamera2D : Camera2D
{
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
        var tree = GetTree();
        var luceNodes = tree.GetNodesInGroup(NodeGroup.Player);
        luce = luceNodes.Cast<Luce>().FirstOrDefault();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (FollowPlayer)
        {
            SetCameraToAdjust();

            if (howLongPlayerMoving.ElapsedMilliseconds > 300/* && howLongPlayerIdle.ElapsedMilliseconds < 2000*/)
            {
                SetPositionOnDirection(delta);
            }
        }
    }
    
    public void SetCameraToAdjust()
    {
        if (!playerIsMoving)
        {
            howLongPlayerIdle.Start();
            
            /*if (howLongPlayerIdle.ElapsedMilliseconds > 2000)
            {
                howLongPlayerMoving.Stop();
                howLongPlayerMoving.Reset();
            }*/
            
            return;
        }
        
        /*howLongPlayerIdle.Stop();
        howLongPlayerIdle.Reset();*/
        
        howLongPlayerMoving.Start();
    }
    
    public void SetPositionOnDirection(double delta)
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
    
    private void MoveNode(Node child, Node parent)
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
    
    // SIGNALS
    public void OnLuceLastWalkDirection(int direction) => lastDirection = (Direction)direction;

    public void OnPlayerIsMoving(bool isMoving) => playerIsMoving = isMoving;

    public void PlayerOnScreenExited()
    {
        FollowPlayer = true;
        MoveNode(this, luce);
    }
}
