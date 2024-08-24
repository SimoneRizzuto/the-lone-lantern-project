using Godot;
using System.Linq;

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

    private OptionButton resolutionOptionButton = new OptionButton();
    private OptionButton windowOptionButton = new OptionButton();
    private OptionButton fpsOptionButton = new OptionButton();



    public override void _Ready()
    {
        // This part should be executed at runtime
        GetTree().Root.ContentScaleMode = Window.ContentScaleModeEnum.Viewport;
        GetTree().Root.ContentScaleAspect = Window.ContentScaleAspectEnum.Expand;
        GetTree().Root.ContentScaleStretch = Window.ContentScaleStretchEnum.Fractional;
        //

        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        
        // Initialise Buttons
        resolutionOptionButton = GetNode<OptionButton>("./MarginContainer/VBoxContainer/SettingsTab/TabContainer/Visual/MarginContainer/VBoxContainer/Res"); // This needs to change & is bad practice
        resolutionOptionButton.ItemSelected += resolutionIndex => ChangeResolution((int) resolutionIndex);
        windowOptionButton = GetNode<OptionButton>("./MarginContainer/VBoxContainer/SettingsTab/TabContainer/Visual/MarginContainer/VBoxContainer/WinScn"); // This needs to change & is bad practice
        windowOptionButton.ItemSelected += windowScreenIndex => ChangeWindowScreen((int) windowScreenIndex);
        fpsOptionButton = GetNode<OptionButton>("./MarginContainer/VBoxContainer/SettingsTab/TabContainer/Visual/MarginContainer/VBoxContainer/FPS"); // This needs to change & is bad practice
        fpsOptionButton.ItemSelected += fpsIndex => ChangeFPSLock((int)fpsIndex);
        
        // Add Options
        AddResolutionOptions();
        AddWindowScreenOptions();
        AddFPSOptions();

        // Auto select Reolution
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

        // Auto select Window Setting
        viewportMode = GetViewport().GetWindow().Mode.ToString();
        if (viewportMode == "ExclusiveFullscreen")
        {
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

        // Auto select FPS
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
        ChangeResolution(resolutionOptionButton.Selected);


    }

    private void ResolutionOptions()
    {
        /*resolutionDict.Add("1024 x 576", new Vector2I(1024, 576));
        resolutionDict.Add("1152 x 648", new Vector2I(1152, 648));*/
        resolutionDict.Add("1280 x 720", new Vector2I(1280, 720));
        resolutionDict.Add("1920 x 1080", new Vector2I(1920, 1080));
        resolutionDict.Add("2560 x 1440", new Vector2I(2560, 1440));
        /*resolutionDict.Add("3840 x 2160", new Vector2I(3840, 2160));*/
    }


    public void AddResolutionOptions()
    {
        ResolutionOptions();
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
