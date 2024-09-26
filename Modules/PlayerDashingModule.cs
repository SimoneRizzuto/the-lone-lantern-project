using System;
using Godot;
using System.Diagnostics;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Modules;

[GlobalClass]
public partial class PlayerDashingModule : Node
{
    [Export] public PlayerStateMachine State;
    [Export] public float DashSpeed = PlayerWalkingModule.DefaultMoveSpeed * 5;
    [Export] public int DashMilliseconds = 1000 / 8;
    
    private readonly Stopwatch sw = new();
    private bool isBufferingNextDash; // do I even want to buffer a dash?? research what other games do... most likely only buffer near the end of a dash
    
    public override void _Ready()
    {
        State ??= GetParent<PlayerStateMachine>();
    }

    public override void _Process(double delta)
    {
        if (State.PlayerState != PlayerState.Dashing)
        {
            sw.Reset();
            isBufferingNextDash = false;
            return;
        }

        if (DashMilliseconds <= sw.Elapsed.Milliseconds)
        {
            if (isBufferingNextDash)
            {
                
            }
            else
            {
                sw.Stop();

                var movementVector = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);
                State.PlayerState = movementVector != Vector2.Zero ? PlayerState.Walking : PlayerState.Idle;
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (!Input.IsActionJustPressed(InputMapAction.Dash)) return;
        
        var directionVector = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);
        if (directionVector != Vector2.Zero)
        {
            if (State.PlayerState is PlayerState.Idle or PlayerState.Walking)
            {
                var dashDirection = DirectionHelper.GetSnappedDirection(directionVector);
                State.Player.CalculatedVelocity = directionVector * DashSpeed;
                State.LastDirection = dashDirection;
                State.PlayerState = PlayerState.Dashing;

                State.DustCloudModule.Play_DashCloud(State.Player.Position);
                
                //State.MainSprite.Play($"");
                
                //start animation
                
                sw.Start();
            }
            
            
            if (DashMilliseconds <= sw.Elapsed.Milliseconds)
            {
                sw.Restart();
            }
            else
            {
                //isBufferingNextDash = true;
            }
        }
    }
}