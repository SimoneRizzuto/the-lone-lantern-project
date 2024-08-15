using Godot;
using TheLoneLanternProject.Constants;

public partial class OptionsMenu : Control
{
    private CustomSignals customSignals = new();

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
    }

    public void OnBackPressed()
    {
        customSignals.EmitSignal(nameof(CustomSignals.MenuSwitch), "Pause");
    }
}
