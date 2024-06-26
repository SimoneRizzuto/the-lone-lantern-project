using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TheLoneLanternProject.Scenes.Player;

public partial class ActorNodeBase : Node2D // ReSharper disable IntroduceOptionalParameters.Global
{
    [Export] public AnimatedSprite2D AnimatedSprite2D;
    [Export] public string MoveUpAnimation;
    [Export] public string MoveRightAnimation;
    [Export] public string MoveLeftAnimation;
    [Export] public string MoveDownAnimation;
    
    private CharacterBody2D actor;
    
    private AsyncActionToPlay asyncActionToPlay = AsyncActionToPlay.NoAction;
    private double millisecondsToPass = 1000;
    private double multiplier = 1;
    private string lastDirection = "down";
    
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
        if (actor != null) return;
        
        var getParentActor = GetParentOrNull<CharacterBody2D>();
        if (getParentActor == null)
        {
            GD.PrintErr($"Cannot find parent {nameof(CharacterBody2D)} as a parent.");
            return;
        }

        actor = getParentActor;
    }

    private async Task SetupActionTask(AsyncActionToPlay asyncAction, double seconds, double? moveSpeedMultiplier = 1)
    {
        actionGiven = new TaskCompletionSource();
        asyncActionToPlay = asyncAction;

        multiplier = moveSpeedMultiplier > 0 ? moveSpeedMultiplier.Value : 1;
        millisecondsToPass = seconds * 1000;
        
        await ActionCompleted;
    }
    
    public virtual void LookUp()
    {
        asyncActionToPlay = AsyncActionToPlay.NoAction;
        lastDirection = "up";
        AnimatedSprite2D.Play($"idle {lastDirection}");
    }
    public virtual void LookRight()
    {
        asyncActionToPlay = AsyncActionToPlay.NoAction;
        lastDirection = "right";
        AnimatedSprite2D.Play($"idle {lastDirection}");
    }
    public virtual void LookLeft()
    {
        asyncActionToPlay = AsyncActionToPlay.NoAction;
        lastDirection = "left";
        AnimatedSprite2D.Play($"idle {lastDirection}");
    }
    public virtual void LookDown()
    {
        asyncActionToPlay = AsyncActionToPlay.NoAction;
        lastDirection = "down";
        AnimatedSprite2D.Play($"idle {lastDirection}");
    }
    
    public virtual void PlayAnimation(string animationToPlay)
    {
        asyncActionToPlay = AsyncActionToPlay.NoAction;
        AnimatedSprite2D.Play(animationToPlay, (float)multiplier);
    }

    public async Task PlayAnimationAsync(string animationToPlay, double playSpeedMultiplier = 1)
    {
        asyncActionToPlay = AsyncActionToPlay.NoAction;
        
        multiplier = playSpeedMultiplier > 0 ? playSpeedMultiplier : 1;
        
        AnimatedSprite2D.Play(animationToPlay, (float)multiplier);

        await ToSignal(AnimatedSprite2D, "animation_finished");
    }
    
    public async Task MoveUp(double seconds = 1, double moveSpeedMultiplier = 1)
    {
        await SetupActionTask(AsyncActionToPlay.MoveUp, seconds, moveSpeedMultiplier);
    }
    public async Task MoveRight(double seconds = 1, double moveSpeedMultiplier = 1)
    {
        await SetupActionTask(AsyncActionToPlay.MoveRight, seconds, moveSpeedMultiplier);
    }
    public async Task MoveLeft(double seconds = 1, double moveSpeedMultiplier = 1)
    {
        await SetupActionTask(AsyncActionToPlay.MoveLeft, seconds, moveSpeedMultiplier);
    }
    public async Task MoveDown(double seconds = 1, double moveSpeedMultiplier = 1)
    {
        await SetupActionTask(AsyncActionToPlay.MoveDown, seconds, moveSpeedMultiplier);
    }
    
    // Process
    public override void _PhysicsProcess(double delta)
    {
        if (asyncActionToPlay == AsyncActionToPlay.NoAction) return;

        if (asyncActionToPlay == AsyncActionToPlay.MoveUp) Move_Process(delta, Vector2.Up);
        if (asyncActionToPlay == AsyncActionToPlay.MoveRight) Move_Process(delta, Vector2.Right);
        if (asyncActionToPlay == AsyncActionToPlay.MoveLeft) Move_Process(delta, Vector2.Left);
        if (asyncActionToPlay == AsyncActionToPlay.MoveDown) Move_Process(delta, Vector2.Down);
    
        if (stopwatch.ElapsedMilliseconds > millisecondsToPass && !ActionCompleted.IsCompleted)
        {
            stopwatch.Stop();
            
            AnimatedSprite2D.Play($"idle {lastDirection}", (float)multiplier);
            asyncActionToPlay = AsyncActionToPlay.NoAction;

            actionGiven.TrySetResult();
        }
    }
    
    public virtual void Move_Process(double delta, Vector2 direction)
    {
        if (!stopwatch.IsRunning)
        {
            stopwatch.Restart();
        }
        
        if (direction == Vector2.Left) lastDirection = "left";
        if (direction == Vector2.Right) lastDirection = "right";
        if (direction == Vector2.Down) lastDirection = "down";
        if (direction == Vector2.Up) lastDirection = "up";
        
        AnimatedSprite2D.Play($"walk {lastDirection}", (float)multiplier);
        actor.Velocity = direction * (PlayerConstants.Speed * (float)multiplier) * (float)delta;
        
        actor.MoveAndSlide();
    }
}

enum AsyncActionToPlay
{
    NoAction,

    Wait,

    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
}
