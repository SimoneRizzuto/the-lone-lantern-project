using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DialogueManagerRuntime;
using Godot;
using TheLoneLanternProject.Scripts.Modules.Weather;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Helpers;
using CastActors = TheLoneLanternProject.Scripts.Shared.Constants.CastActors;
using CustomSignals = TheLoneLanternProject.Scripts.Utils.Signals.CustomSignals;

namespace TheLoneLanternProject.Scripts.Directors.Cutscene;
public partial class CutsceneDirector : Node
{
    private CustomSignals customSignals = new();
    private Player.Luce luce = new();
    private ActorNodeBase luceActor = new();
    private WeatherModule weatherModule;
    
    private AsyncActionToPlay asyncActionToPlay = AsyncActionToPlay.NoAction;
    private double millisecondsToPass = 1000;
    private double multiplier = 1;
    private string lastDirection = "down";
    
    // variables for managing time passing for Action commands
    private Task ActionCompleted => actionGiven.Task;
    private TaskCompletionSource actionGiven = new();
    private readonly Stopwatch stopwatch = new();
    
    
    
    /*
     * There are two options for implementing Cutscene directions:
     * 1. Use AnimationPlayer to build out custom animations. This is valuable for more complex animations.
     * 2. Implement generic commands for nodes that can be easily used using signals inside of a .dialogue file.
     *
     * The second one may be more complex, as I will need to figure out a clean way to point to a node that required a command.
     * I found this tutorial: https://www.youtube.com/watch?v=U3Mimia-904&ab_channel=NathanHoad
     * Using this method of giving an actor a command, I would want to make a generic version
     * of this so that I can attach it to every node that requires.
     * e.g. Make an Actor class, attach it to each node that needs to be an Actor. If any Actors have more complex actions, extend it.
     *
     *
     */
    
    public override void _Ready()
    {
        if (!LoadActorsIntoCurrentScene()) return;
        
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.ShowDialogueBalloon += ShowDialogueBalloon;

        var tree = GetTree();
        luce = GetNodeHelper.GetLuce(tree);
        var actorNodes = tree.GetNodesInGroup(NodeGroup.ActorNode);
        var actorBaseNodes = actorNodes.Cast<ActorNodeBase>().ToList();

        foreach (var actor in actorBaseNodes)
        {
            if (actor.Actor?.Name == ActorNames.Luce)
            {
                luceActor = actor;
            }
        }
    }
    private bool LoadActorsIntoCurrentScene()
    {
        var scriptPath = $"res://Scripts/Shared/Constants/{nameof(CastActors)}.cs";
        try
        {
            var scriptLoaded = ResourceLoader.Load(scriptPath);
            var scriptAsVariant = (Variant)scriptLoaded;

            var currentScene = GetTree().CurrentScene;
            var script = GetTree().CurrentScene.GetScript();
            if (script.Obj != null)
            {
                GD.PrintErr($"Script {script.Obj} already exists in the CurrentScene \"{currentScene.Name}\". Cannot load \"{scriptPath}\" as a script. Actor directions may stop functioning.");
                return false;
            }
        
            GetTree().CurrentScene.SetScript(scriptAsVariant);
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Cannot load \"{scriptPath}\" as a script. Actor directions may stop functioning. ${ex.Message}");
        }

        return true;
    }
    private void ShowDialogueBalloon(string dialogue, string title)
    {
        luce.SetPlayerState(PlayerState.Disabled);
        
        DialogueManager.ShowDialogueBalloon(GD.Load($"res://Assets/Dialogue/{dialogue}.dialogue"), title);
        DialogueManager.DialogueEnded += SetupGameplayAfterDialogueEnded;
    }
    
    private async Task SetupActionTask(AsyncActionToPlay asyncAction, double seconds, double? moveSpeedMultiplier = 1)
    {
        actionGiven = new TaskCompletionSource();
        asyncActionToPlay = asyncAction;

        multiplier = moveSpeedMultiplier > 0 ? moveSpeedMultiplier.Value : 1;
        millisecondsToPass = seconds * 1000;
        
        await ActionCompleted;
    }
    
    public virtual async Task Wait(double seconds = 1)
    {
        await SetupActionTask(AsyncActionToPlay.Wait, seconds);
    }
    
    public void TriggerRain()
    {
        if (weatherModule == null)
        {
            weatherModule = GetNodeHelper.GetWeatherModule(GetTree());
        }
        else
        {
            weatherModule = InstanceNodeHelper.Rain(this);
        }

        weatherModule.TriggerRain();
        // emit signal instead
    }
    
    public void StopRain()
    {
        if (weatherModule == null)
        {
            weatherModule = GetNodeHelper.GetWeatherModule(GetTree());
        }
        else
        {
            weatherModule = InstanceNodeHelper.Rain(this);
        }
        
        weatherModule.StopRain();
        // emit signal instead
    }
    
    public async Task CheckForAttackInput()
    {
        await SetupActionTask(AsyncActionToPlay.WaitForInput, 3);
    }

    private void FinishTask()
    {
        stopwatch.Stop();
        asyncActionToPlay = AsyncActionToPlay.NoAction;
        actionGiven.TrySetResult();
    }
    
    // Flags
    public bool noriReapTriggered; // used in DialogueManager for branching the dialogue
    
    public override void _Process(double delta)
    {
        if (asyncActionToPlay == AsyncActionToPlay.NoAction) return;
        if (asyncActionToPlay == AsyncActionToPlay.WaitForInput)
        {
            // logic for wait, logic for returning when input is received
            if (Input.IsActionJustPressed(InputMapAction.Attack))
            {
                noriReapTriggered = true;
                FinishTask();
            }
        }
        
        if (!stopwatch.IsRunning)
        {
            stopwatch.Restart();
        }
        
        if (stopwatch.ElapsedMilliseconds > millisecondsToPass)
        {
            FinishTask();
        }
    }
    
    private void SetupGameplayAfterDialogueEnded(Resource dialogueResource)
    {
        luce.SetPlayerState(PlayerState.Idle);
        DialogueManager.DialogueEnded -= SetupGameplayAfterDialogueEnded;
    }
}