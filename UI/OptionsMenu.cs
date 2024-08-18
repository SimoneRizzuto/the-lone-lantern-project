using Godot;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

public partial class OptionsMenu : Control
{
    private CustomSignals customSignals = new();
    private int viewportHeight = new();
    private int viewportWidth = new();
    private int engineFPS= 60;
    private string viewportMode = "";

    private Godot.Collections.Dictionary<string, Vector2I> resolutionDict = new();
    private Godot.Collections.Dictionary<string, int> fpsDict = new();
    private Godot.Collections.Array<string> windowScreenArray = new();



    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        
        AddResolutionOptions();
        OptionButton resolutionOptionButton = GetNode<OptionButton>("./MarginContainer/VBoxContainer/SettingsTab/TabContainer/Visual/MarginContainer/VBoxContainer/Res"); // This needs to change & is bad practice
        resolutionOptionButton.ItemSelected += resolutionIndex => ChangeResolution((int) resolutionIndex);

        viewportHeight = (int)ProjectSettings.GetSetting("display/window/size/viewport_height");
        viewportWidth = (int)ProjectSettings.GetSetting("display/window/size/viewport_width");
        string key = $"{viewportWidth} x {viewportHeight}";


        if (resolutionDict.ContainsKey(key))
        {
            resolutionOptionButton.Selected = resolutionDict.Keys.ToList().IndexOf(key);
        }
        else
        {
            resolutionOptionButton.Selected = -1; 

        }

        AddWindowScreenOptions();
        OptionButton windowOptionButton = GetNode<OptionButton>("./MarginContainer/VBoxContainer/SettingsTab/TabContainer/Visual/MarginContainer/VBoxContainer/WinScn"); // This needs to change & is bad practice
        windowOptionButton.ItemSelected += windowScreenIndex => ChangeWindowScreen((int)windowScreenIndex);

        viewportMode = GetViewport().GetWindow().Mode.ToString();
        if (viewportMode == "ExclusiveFullscreen"){
            viewportMode = "Exclusive Fullscreen";
        }
        if (windowScreenArray.Contains(viewportMode))
        {
            
            windowOptionButton.Selected = windowScreenArray.IndexOf(viewportMode); 
        }
        else
        {
            windowOptionButton.Selected = -1;
        }

        AddFPSOptions();
        OptionButton fpsOptionButton = GetNode<OptionButton>("./MarginContainer/VBoxContainer/SettingsTab/TabContainer/Visual/MarginContainer/VBoxContainer/FPS"); // This needs to change & is bad practice
        fpsOptionButton.ItemSelected += fpsIndex => ChangeFPSLock((int)fpsIndex);
        Engine.MaxFps = engineFPS; // By default
        fpsOptionButton.Selected = fpsDict.Keys.ToList().IndexOf("60 FPS");
    }


    private void WindowScreenOptions()
    {
        windowScreenArray.Add("Windowed");
        windowScreenArray.Add("Fullscreen");
        windowScreenArray.Add("Exclusive Fullscreen");

    }

    public void AddWindowScreenOptions()
    {
        WindowScreenOptions();
        var windowOptionButton = GetNode<OptionButton>("./MarginContainer/VBoxContainer/SettingsTab/TabContainer/Visual/MarginContainer/VBoxContainer/WinScn"); // This needs to change & is bad practice
        //GD.Print(GetTree().Root.GetTreeString());
        foreach (var screen in windowScreenArray)
        {
            windowOptionButton.AddItem(screen);
        }
    }

    private void ChangeWindowScreen(int windowScreenIndex)
    {
        switch (windowScreenIndex)
        {
            case 0:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed); 
                break;
            case 1:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                break;
            case 2:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
                break;
            default:
                break;
        }
    }

    private void ResolutionOptions()
    {
        resolutionDict.Add("1024 x 576", new Vector2I(1024, 576));
        resolutionDict.Add("1152 x 648", new Vector2I(1152, 648));
        resolutionDict.Add("1280 x 720", new Vector2I(1280, 720));
        resolutionDict.Add("1345 x 790", new Vector2I(1345, 790)); // Let's add a weird one to check the scaling
        resolutionDict.Add("1920 x 1080", new Vector2I(1920, 1080));
        resolutionDict.Add("2560 x 1440", new Vector2I(2560, 1440));
        resolutionDict.Add("3840 x 2160", new Vector2I(3840, 2160));
    }


    public void AddResolutionOptions()
    {
        ResolutionOptions();
        var resolutionOptionButton = GetNode<OptionButton>("./MarginContainer/VBoxContainer/SettingsTab/TabContainer/Visual/MarginContainer/VBoxContainer/Res"); // This needs to change & is bad practice
        //GD.Print(GetTree().Root.GetTreeString());
        foreach (var resolution in resolutionDict)
        {
            resolutionOptionButton.AddItem(resolution.Key);
        }
    }

    private void ChangeResolution(int resolutionIndex)
    {
        DisplayServer.WindowSetSize(resolutionDict.Values.ToList()[resolutionIndex]);
    }

    private void FPSOptions()
    {
        fpsDict.Add("30 FPS", 30);
        fpsDict.Add("60 FPS", 60);
    }

    private void AddFPSOptions()
    {
        FPSOptions();
        var fpsOptionButton = GetNode<OptionButton>("./MarginContainer/VBoxContainer/SettingsTab/TabContainer/Visual/MarginContainer/VBoxContainer/FPS"); // This needs to change & is bad practice
        foreach (var fps in fpsDict)
        {
            fpsOptionButton.AddItem(fps.Key);
        }
    }

    private void ChangeFPSLock(int fpsIndex)
    {
        Engine.MaxFps = fpsDict.Values.ToList()[fpsIndex]; 
    }

    public void OnBackPressed()
    {
        customSignals.EmitSignal(nameof(CustomSignals.MenuSwitch), "Pause");
    }

}
