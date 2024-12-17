using System.Linq;
using Godot;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Helpers;
public static class GetNodeHelper
{
    public static Luce3 GetLuce(SceneTree tree)
    {
        var luceNodes = tree.GetNodesInGroup(NodeGroup.Player);
        var luce = luceNodes.Cast<Luce3>().FirstOrDefault();
        if (luce == null)
        {
            GD.PrintErr($"{nameof(luce)} was null.");
        }
        
        return luce;
    }
    
    public static MainCamera2D GetMainCamera2D(SceneTree tree)
    {
        var mainCamera2DNodes = tree.GetNodesInGroup(NodeGroup.MainCamera);
        var mainCamera2D = mainCamera2DNodes.Cast<MainCamera2D>().FirstOrDefault();
        if (mainCamera2D == null)
        {
            GD.PrintErr($"{nameof(mainCamera2D)} was null.");
        }
        
        return mainCamera2D;
    }
    
    public static PlayerCamera2D GetPlayerCamera2D(SceneTree tree)
    {
        var playerCamera2DNodes = tree.GetNodesInGroup(NodeGroup.PlayerCamera);
        var playerCamera2D = playerCamera2DNodes.Cast<PlayerCamera2D>().FirstOrDefault();
        if (playerCamera2D == null)
        {
            GD.PrintErr($"{nameof(playerCamera2D)} was null.");
        }
        
        return playerCamera2D;
    }
    
    public static Rain GetRain(SceneTree tree)
    {
        var weatherNodes = tree.GetNodesInGroup(NodeGroup.Weather);
        var rain = weatherNodes.Cast<Rain>().FirstOrDefault();
        if (rain == null)
        {
            GD.PrintErr($"{nameof(rain)} was null.");
        }

        return rain;
    }
}