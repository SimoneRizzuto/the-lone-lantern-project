using Godot;
using System;
using System.Linq;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Helpers;

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
    
    public override void _Ready()
    {
        var tree = GetTree();
        
        var cutsceneDirectorNodes = tree.GetNodesInGroup(NodeGroup.CutsceneDirector);
        director = cutsceneDirectorNodes.Cast<CutsceneDirector>().FirstOrDefault();
        
        camera = GetNodeHelper.GetMainCamera2D(tree);
        
        var actorNodes = tree.GetNodesInGroup(NodeGroup.ActorNode);
        luce = actorNodes.Cast<ActorNodeBase>().FirstOrDefault();
    }
}
