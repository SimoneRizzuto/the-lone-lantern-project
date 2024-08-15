using Godot;
using TheLoneLanternProject.Constants;

public partial class UI : CanvasLayer
{
    private Control optionsMenu = new();
    private Control pauseMenu = new();
    private CustomSignals customSignals = new();


    public override void _Ready()
    {
        // Select particular UI to show.
        pauseMenu = GetNode<Control>("./PauseMenu");
        /*optionsMenu = GetNode<Control>("./OptionsMenu");*/
        /*optionsMenu.Visible = false;*/
        pauseMenu.Visible = false;

        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.MenuSwitch += HandleMenuSwitch;

    }


    private void HandleMenuSwitch(string menu) 
    {
        RemoveChild(GetChild(0));
        
        var newMenu = ResourceLoader.Load< PackedScene>($"res://UI//{menu}Menu.tscn").Instantiate();
        AddChild(newMenu);
    }


    /*public void OnOptionsMenuGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex == MouseButton.Left && mb.Pressed) { }
        }
    }

    public void OnPauseMenuGuiInput(InputEvent @event) 
    { 

    }*/
}

