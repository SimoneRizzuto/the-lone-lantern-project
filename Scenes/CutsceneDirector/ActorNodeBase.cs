using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TheLoneLanternProject.Scenes.Player;

public partial class ActorNodeBase : Node2D // ReSharper disable IntroduceOptionalParameters.Global
{
    [Export] public AnimatedSprite2D MainSprite;
    [Export] public string MoveUpAnimation;
    [Export] public string MoveRightAnimation;
    [Export] public string MoveLeftAnimation;
    [Export] public string MoveDownAnimation;
    
    private CharacterBody2D actor;
    
    private ActionToPlay actionToPlay = ActionToPlay.NoAction;
    private double millisecondsToPass = 1000;
    private double multiplier = 1;
    
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
        if (this.actor != null) return;
        
        var actor = GetParentOrNull<CharacterBody2D>();
        if (actor == null)
        {
            GD.PrintErr($"Cannot find parent {nameof(CharacterBody2D)} as a parent.");
            return;
        }

        this.actor = actor;
    }

    private async Task SetupActionTask(ActionToPlay action, double seconds, double? moveSpeedMultiplier = 1)
    {
        actionGiven = new TaskCompletionSource();
        actionToPlay = action;

        multiplier = moveSpeedMultiplier > 0 ? moveSpeedMultiplier.Value : 1;
        millisecondsToPass = seconds * 1000;
        
        await ActionCompleted;
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
    
    public virtual async Task Wait(double seconds = 1)
    {
        await SetupActionTask(ActionToPlay.Wait, seconds);
    }
    public async Task MoveUp(double seconds = 1, double moveSpeedMultiplier = 1)
    {
        await SetupActionTask(ActionToPlay.MoveUp, seconds, moveSpeedMultiplier);
    }
    public async Task MoveRight(double seconds = 1, double moveSpeedMultiplier = 1)
    {
        await SetupActionTask(ActionToPlay.MoveRight, seconds, moveSpeedMultiplier);
    }
    public async Task MoveLeft(double seconds = 1, double moveSpeedMultiplier = 1)
    {
        await SetupActionTask(ActionToPlay.MoveLeft, seconds, moveSpeedMultiplier);
    }
    public async Task MoveDown(double seconds = 1, double moveSpeedMultiplier = 1)
    {
        await SetupActionTask(ActionToPlay.MoveDown, seconds, moveSpeedMultiplier);
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
        
        if (stopwatch.ElapsedMilliseconds > millisecondsToPass)
        {
            stopwatch.Stop();
            
            var animationDirection = "";
            if (actionToPlay == ActionToPlay.MoveLeft) animationDirection = "left";
            if (actionToPlay == ActionToPlay.MoveRight) animationDirection = "right";
            if (actionToPlay == ActionToPlay.MoveDown) animationDirection = "down";
            if (actionToPlay == ActionToPlay.MoveUp) animationDirection = "up";
            
            MainSprite.Animation = $"idle {animationDirection}";
            
            actionToPlay = ActionToPlay.NoAction;

            actionGiven.TrySetResult();
        }
    }
    
    public virtual void Move_Process(double delta, Vector2 direction)
    {
        if (!stopwatch.IsRunning)
        {
            stopwatch.Restart();
        }
        
        var animationDirection = "";
        if (direction == Vector2.Left) animationDirection = "left";
        if (direction == Vector2.Right) animationDirection = "right";
        if (direction == Vector2.Down) animationDirection = "down";
        if (direction == Vector2.Up) animationDirection = "up";

        MainSprite.Animation = $"walk {animationDirection}";
        MainSprite.SpeedScale = (float)multiplier;
        if (!MainSprite.IsPlaying()) MainSprite.Play();

        actor.Velocity = direction * (PlayerConstants.Speed * (float)multiplier) * (float)delta;
        
        actor.MoveAndSlide();
    }
}

enum ActionToPlay
{
    NoAction,
    
    LookUp,
    LookDown,
    LookLeft,
    LookRight,
    
    Wait,
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    
    PlayAnimation,
    //PlaySound,
    //PlayAnimationSound,
}
