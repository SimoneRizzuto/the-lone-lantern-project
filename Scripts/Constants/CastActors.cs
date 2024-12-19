using System.Linq;
using Godot;
using TheLoneLanternProject.Scripts.Directors;
using TheLoneLanternProject.Scripts.Helpers;

namespace TheLoneLanternProject.Scripts.Constants;

/// <summary>
/// When a new Actor needs to be available for a .dialogue file to direct it, add it as a property in CastActors.
/// This will allow Dialogue Manager access to it and its' generic Action methods.
/// </summary>
public partial class CastActors : Node
{
    // Cast
    public Directors.Cutscene.CutsceneDirector director;
    public AudioDirector audioDirector;
    public Camera.MainCamera2D camera;
    
    // Actors
    public Directors.Cutscene.ActorNodeBase luce;
    public Directors.Cutscene.ActorNodeBase nori;
    
    public override void _Ready()
    {
        var tree = GetTree();

        var cutsceneDirectorNodes = tree.GetNodesInGroup(NodeGroup.CutsceneDirector);
        director = cutsceneDirectorNodes.Cast<Directors.Cutscene.CutsceneDirector>().FirstOrDefault();

        audioDirector = AudioDirector.Instance;

        camera = GetNodeHelper.GetMainCamera2D(tree);
        
        var actorNodes = tree.GetNodesInGroup(NodeGroup.ActorNode);
        var actorBaseNodes = actorNodes.Cast<Directors.Cutscene.ActorNodeBase>().ToList();

        foreach (var actor in actorBaseNodes)
        {
            if (actor.Actor.Name == ActorNames.Luce)
            {
                luce = actor;
            }

            if (actor.Actor.Name == ActorNames.Nori)
            {
                nori = actor;
            }
        }
    }
}

// Actor Names
public static class ActorNames
{
    public static string Luce = "Luce";
    public static string Nori = "Nori";
}