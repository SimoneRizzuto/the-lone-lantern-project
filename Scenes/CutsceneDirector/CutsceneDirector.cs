using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DialogueManagerRuntime;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

public partial class CutsceneDirector : Node
{
    private CustomSignals customSignals = new();
    private Player player = new();
    
    private ActionToPlay actionToPlay = ActionToPlay.NoAction;
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
        var playerNodes = tree.GetNodesInGroup(NodeGroup.Player);
        player = playerNodes.Cast<Player>().FirstOrDefault();
    }
    private bool LoadActorsIntoCurrentScene()
    {
        var scriptPath = $"res://Constants/{nameof(CastActors)}.cs";
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
        player.State = PlayerState.Disabled;
        
        DialogueManager.ShowDialogueBalloon(GD.Load($"res://Dialogue/{dialogue}.dialogue"), title);
    }
    
    private async Task SetupActionTask(ActionToPlay action, double seconds, double? moveSpeedMultiplier = 1)
    {
        actionGiven = new TaskCompletionSource();
        actionToPlay = action;

        multiplier = moveSpeedMultiplier > 0 ? moveSpeedMultiplier.Value : 1;
        millisecondsToPass = seconds * 1000;
        
        await ActionCompleted;
    }
    
    public virtual async Task Wait(double seconds = 1)
    {
        await SetupActionTask(ActionToPlay.Wait, seconds);
    }
    
    public override void _Process(double delta)
    {
        if (!stopwatch.IsRunning)
        {
            stopwatch.Restart();
        }
        
        if (stopwatch.ElapsedMilliseconds > millisecondsToPass)
        {
            stopwatch.Stop();

            actionToPlay = ActionToPlay.NoAction;

            actionGiven.TrySetResult();
        }
    }
}
