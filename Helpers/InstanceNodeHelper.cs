using Godot;

namespace TheLoneLanternProject.Helpers;

public static class InstanceNodeHelper
{
    public static Node InstanceNode(Node tree, string uidString)
    {
        var uid = ResourceUid.TextToId(uidString);
        var path = ResourceUid.GetIdPath(uid);
        var packedScene = (PackedScene)ResourceLoader.Load(path);
        
        var sceneToAdd = packedScene.Instantiate();
        tree.AddChild(sceneToAdd);

        return sceneToAdd;
    }
    
    public static Rain Rain(Node tree)
    {
        var node = InstanceNode(tree, "uid://buj8skc4edik3");
        return (Rain)node;
    }
}