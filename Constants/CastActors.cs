using Godot;
using System;
using System.Linq;
using TheLoneLanternProject.Constants;

/// <summary>
/// When a new Actor needs to be available for a .dialogue file to direct it, add it as a property in CastActors.
/// This will allow Dialogue Manager access to it and its' generic Action methods.
/// </summary>
public partial class CastActors : Node
{
    public CutsceneDirector director;
    
    public ActorNodeBase luce;
    
    
    
    public override void _Ready()
    {
        var tree = GetTree();
        
        var actorNodes = tree.GetNodesInGroup(NodeGroup.ActorNode);
        luce = actorNodes.Cast<ActorNodeBase>().FirstOrDefault();
        
        var cutsceneDirectorNodes = tree.GetNodesInGroup(NodeGroup.CutsceneDirector);
        director = cutsceneDirectorNodes.Cast<CutsceneDirector>().FirstOrDefault();
    }
}
