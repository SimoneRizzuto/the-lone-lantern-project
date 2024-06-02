using Godot;
using System;

namespace TheLoneLanternProject.Scenes.SaveHelper;
public partial class SaveHelper : Node
{
    public static Godot.Collections.Dictionary<string, Variant> Save()
    {
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            {"CurrentScene","2"},
            //{"Health": },
        };
    }
}