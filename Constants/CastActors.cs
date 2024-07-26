using Godot;
using System.Linq;
using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Constants;

/// <summary>
/// When a new Actor needs to be available for a .dialogue file to direct it, add it as a property in CastActors.
/// This will allow Dialogue Manager access to it and its' generic Action methods.
/// </summary>
public partial class CastActors : Node
{
    // Cast
    public CutsceneDirector director;
    public MainCamera2D camera;
    
    // Actors
    public ActorNodeBase luce;
    public ActorNodeBase nori;
    
    public override void _Ready()
    {
        var tree = GetTree();
        
        var cutsceneDirectorNodes = tree.GetNodesInGroup(NodeGroup.CutsceneDirector);
        director = cutsceneDirectorNodes.Cast<CutsceneDirector>().FirstOrDefault();
        
        camera = GetNodeHelper.GetMainCamera2D(tree);
        
        var actorNodes = tree.GetNodesInGroup(NodeGroup.ActorNode);
        var actorBaseNodes = actorNodes.Cast<ActorNodeBase>().ToList();

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
        
        //luce = actorBaseNodes.FirstOrDefault();
    }
}

// Actor Names
public static class ActorNames
{
    public static string Luce = "Luce";
    public static string Nori = "Nori";
}
