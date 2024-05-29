using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TheLoneLanternProject.Scenes.Player;


public partial class ActorNode : Node2D // ReSharper disable IntroduceOptionalParameters.Global
{
    [Export] public CharacterBody2D Actor;
    
    private ActionToPlay actionToPlay = ActionToPlay.NoAction;

    private double millisecondsToPass = 1000;
    
    // variables for managing time passing for Action commands
    private Task ActionCompleted => actionGiven.Task;
    private TaskCompletionSource actionGiven = new();
    private readonly Stopwatch stopwatch = new();

    public override void _Ready()
    {
        FindParentActor();
    }
    private void FindParentActor()
    {
        if (Actor != null) return;
        
        var actor = GetParentOrNull<CharacterBody2D>();
        if (actor == null)
        {
            GD.PrintErr($"Cannot find parent {nameof(CharacterBody2D)} as a parent.");
            return;
        }

        Actor = actor;
    }

    private async Task SetupAction(ActionToPlay action, double seconds)
    {
        actionGiven = new TaskCompletionSource();
        actionToPlay = action;
        
        millisecondsToPass = seconds * 1000;
        
        await ActionCompleted;
    }
    
    public virtual async Task Wait(double seconds = 1)
    {
        await SetupAction(ActionToPlay.Wait, seconds);
    }
    
    public virtual void LookUp()
    {
        actionToPlay = ActionToPlay.LookUp;
    }
    public virtual void LookRight()
    {
        actionToPlay = ActionToPlay.LookRight;
    }
    public virtual void LookLeft()
    {
        actionToPlay = ActionToPlay.LookLeft;
    }
    public virtual void LookDown()
    {
        actionToPlay = ActionToPlay.LookDown;
    }
    
    public async Task MoveUp(double seconds = 1)
    {
        await SetupAction(ActionToPlay.MoveUp, seconds);
    }
    public async Task MoveRight(double seconds = 1)
    {
        await SetupAction(ActionToPlay.MoveRight, seconds);
    }
    public async Task MoveLeft(double seconds = 1)
    {
        await SetupAction(ActionToPlay.MoveLeft, seconds);
    }
    public async Task MoveDown(double seconds = 1)
    {
        await SetupAction(ActionToPlay.MoveDown, seconds);
    }
    
    // Process
    
    public override void _PhysicsProcess(double delta)
    {
        if (actionToPlay == ActionToPlay.NoAction) return;
        
        if (actionToPlay == ActionToPlay.Wait) Move_Process(delta, Vector2.Zero);
        if (actionToPlay == ActionToPlay.MoveUp) Move_Process(delta, Vector2.Up);
        if (actionToPlay == ActionToPlay.MoveRight) Move_Process(delta, Vector2.Right);
        if (actionToPlay == ActionToPlay.MoveLeft) Move_Process(delta, Vector2.Left);
        if (actionToPlay == ActionToPlay.MoveDown) Move_Process(delta, Vector2.Down);
    }
    
    private void Move_Process(double delta, Vector2 direction)
    {
        if (!stopwatch.IsRunning)
        {
            stopwatch.Restart();
        }
        
        Actor.Velocity = direction * PlayerConstants.Speed * (float)delta;
        Actor.MoveAndSlide();
        
        if (stopwatch.ElapsedMilliseconds > millisecondsToPass)
        {
            stopwatch.Stop();
            actionToPlay = ActionToPlay.NoAction;
            
            actionGiven.SetResult();
        }
    }
}

enum ActionToPlay
{
    NoAction,
    
    Wait,
    
    LookUp,
    LookDown,
    LookLeft,
    LookRight,

    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    
    PlayAnimation,
}
