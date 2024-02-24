using Godot;
using System;

public partial class StaminaHealthBar : CanvasLayer
{
    private TextureProgressBar progressBar;
    public override void _Ready()
    {
        progressBar = GetNode<TextureProgressBar>("Container/TextureProgressBar");
        progressBar.Value = progressBar.MaxValue;
    }

    // SIGNALS
    public void OnPlayerHealthChanged(double newHealth)
    {
        progressBar.Value = newHealth;
    }
}