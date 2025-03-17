using Godot;

namespace TheLoneLanternProject.Scripts.Modules.Bars.StaminaHealth;
public partial class StaminaHealthBar : CanvasLayer
{
    private TextureProgressBar progressBar;
    public override void _Ready()
    {
        progressBar = GetNode<TextureProgressBar>("Container/TextureProgressBar");
        progressBar.Value = progressBar.MaxValue;
    }

    public void SetValue(double newHealth)
    {
        progressBar.Value = newHealth;
    }

    // SIGNALS
    public void OnPlayerHealthChanged(double newHealth)
    {
        progressBar.Value = newHealth;
    }
}