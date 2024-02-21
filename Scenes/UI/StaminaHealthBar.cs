using Godot;
using System;

public partial class StaminaHealthBar : CanvasLayer
{
    private TextureProgressBar progressBar;
    private Timer regenBuffer;
    
    private State state = State.Max;

    private double refillSpeed = 50;
    
    public override void _Ready()
    {
        progressBar = GetNode<TextureProgressBar>("Container/TextureProgressBar");
        regenBuffer = GetNode<Timer>("RegenBuffer");

        progressBar.Value = progressBar.MaxValue;
    }

    public override void _Process(double delta)
    {
        if (state == State.Regen)
        {
            var progressBarNextValue = 1 * refillSpeed * delta;
            progressBar.Value += progressBarNextValue;
            
            Console.WriteLine(progressBarNextValue);
        }
    }

    public void SetValue(double value, State stateToSet)
    {
        progressBar.Value += value;
        state = stateToSet;
    }

    public void StartRefilling()
    {
        state = State.Regen;
    }

    // SIGNALS
    public void OnPlayerAttack()
    {
        SetValue(-20, State.Pause);
        regenBuffer.Start();
    }

    public void OnRegenBufferTimeout()
    {
        StartRefilling();
    }
}

public enum State
{
    Max = 0,
    Pause = 1,
    Regen = 2
}