using System.Linq;
using Godot;
using TheLoneLanternProject.Constants;

namespace TheLoneLanternProject.Scenes.Player;

public static class LuceHelper
{
    public static Luce GetLuce(SceneTree tree)
    {
        var luceNodes = tree.GetNodesInGroup(NodeGroup.Player);
        var luce = luceNodes.Cast<Luce>().FirstOrDefault();
        if (luce == null)
        {
            GD.PrintErr($"{nameof(luce)} was null.");
        }

        return luce;
    }
}