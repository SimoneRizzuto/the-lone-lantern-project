using System.Linq;
using Godot;
using TheLoneLanternProject.Scripts.Modules.Camera;
using TheLoneLanternProject.Scripts.Modules.Weather;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Shared.Helpers;
public static class GetNodeHelper
{
    public static Player.Luce GetLuce(SceneTree tree)
    {
        var luceNodes = tree.GetNodesInGroup(NodeGroup.Player);
        var luce = luceNodes.Cast<Player.Luce>().FirstOrDefault();
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
    
    public static WeatherModule GetWeatherModule(SceneTree tree)
    {
        var weatherNodes = tree.GetNodesInGroup(NodeGroup.Weather);
        var rain = weatherNodes.Cast<WeatherModule>().FirstOrDefault();
        if (rain == null)
        {
            GD.PrintErr($"{nameof(rain)} was null.");
        }

        return rain;
    }
}