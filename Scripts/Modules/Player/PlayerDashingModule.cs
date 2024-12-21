using System.Diagnostics;
using Godot;
using TheLoneLanternProject.Modules;
using TheLoneLanternProject.Scripts.Constants;
using TheLoneLanternProject.Scripts.Helpers;

namespace TheLoneLanternProject.Scripts.Modules.Player;

[GlobalClass]
public partial class PlayerDashingModule : Node
{
    [Export] public StateMachines.Player.PlayerStateMachine State;
    [Export] public float DashSpeed = PlayerWalkingModule.DefaultMoveSpeed * 5;
    [Export] public int DashLengthMilliseconds = 1000 / 8;

    private bool AllowDash => State.StaminaHealthModule.AllowAction;

    private readonly Stopwatch nextDashDelay = new();
    
    private readonly Stopwatch sw = new();
    private bool isBufferingNextDash; // do I even want to buffer a dash?? research what other games do... most likely only buffer near the end of a dash
    
    public override void _Ready()
    {
        State ??= GetParent<StateMachines.Player.PlayerStateMachine>();
    }

    public override void _Process(double delta)
    {
        if (State.PlayerState != PlayerState.Dashing)
        {
            sw.Reset();
            isBufferingNextDash = false;
            return;
        }

        if (DashLengthMilliseconds <= sw.Elapsed.Milliseconds)
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
        if (!Input.IsActionJustPressed(InputMapAction.Dash) || !AllowDash) return;
        
        if (nextDashDelay.ElapsedMilliseconds <= 2000 && nextDashDelay.ElapsedMilliseconds != 0)
        {
            nextDashDelay.Reset();
            return;
        }
        
        var directionVector = Input.GetVector(InputMapAction.Left, InputMapAction.Right, InputMapAction.Up, InputMapAction.Down);
        if (directionVector == Vector2.Zero) return;
        
        if (State.PlayerState is PlayerState.Idle or PlayerState.Walking)
        {
            var dashDirection = DirectionHelper.GetSnappedDirection(directionVector);
            State.Player.CalculatedVelocity = directionVector * DashSpeed;
            State.LastDirection = dashDirection;
            
            State.PlayerState = PlayerState.Dashing;
            State.StaminaHealthModule.RemoveStaminaHealth(15);

            State.DustCloudModule.Play_DashCloud(State.Player.Position);
            
            //State.MainSprite.Play($"");
            
            //start animation
            
            sw.Start();
        }
        
        nextDashDelay.Restart();
        
        if (DashLengthMilliseconds <= sw.Elapsed.Milliseconds)
        {
            sw.Restart();
        }
        else
        {
            //isBufferingNextDash = true;
        }
    }
}