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

    private async Task SetupAction(ActionToPlay action)
    {
        actionGiven = new TaskCompletionSource();
        actionToPlay = action;
        
        await ActionCompleted;
    }
    
    public virtual async Task Wait()
    {
        await SetupAction(ActionToPlay.Wait);
    }
    
    public virtual async Task LookUp()
    {
        await SetupAction(ActionToPlay.LookUp);
    }
    
    public virtual async Task LookRight()
    {
        await SetupAction(ActionToPlay.LookRight);
    }
    
    public virtual async Task LookLeft()
    {
        await SetupAction(ActionToPlay.LookLeft);
    }
    
    public virtual async Task LookDown()
    {
        await SetupAction(ActionToPlay.LookDown);
    }
    
    public async Task MoveUp()
    {
        await SetupAction(ActionToPlay.MoveUp);
    }
    
    public async Task MoveRight()
    {
        await SetupAction(ActionToPlay.MoveRight);
    }
    
    public async Task MoveLeft()
    {
        await SetupAction(ActionToPlay.MoveLeft);
    }
    
    public async Task MoveDown()
    {
        await SetupAction(ActionToPlay.MoveDown);
    }
    
    // Process
    
    public override void _PhysicsProcess(double delta)
    {
        if (actionToPlay == ActionToPlay.NoAction) return;
        
        
        if (actionToPlay == ActionToPlay.Wait) Move_Process(delta, Vector2.Zero);
        
        //if (actionToPlay == ActionToPlay.LookUp) LookUp_Process(delta);
        //if (actionToPlay == ActionToPlay.LookRight) LookRight_Process(delta);
        //if (actionToPlay == ActionToPlay.LookLeft) LookLeft_Process(delta);
        //if (actionToPlay == ActionToPlay.LookDown) LookDown_Process(delta);
        
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
        
        if (stopwatch.ElapsedMilliseconds > 500) // 1 second = 1000
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
