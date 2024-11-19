using Godot;
using System;
using System.Diagnostics;
using TheLoneLanternProject.Scenes.PlayerController;

public partial class StaminaHealthModule : Node2D
{
	private double h = 0;
	private double Health
	{
		get => h;
		set
		{
			h = value;
			
			// variable set for visible bar
			bar.SetValue(value);
			bar.Visible = true;
		}
	}
	
	private readonly double maxHealth = 100;

	private Stopwatch regenBufferTimer = new();
	private double regenBufferSeconds = 3;
	private bool isRegenerating;
	
	private StaminaHealthBar bar;
	private Stopwatch visibilityTimer = new();
	
	public override void _Ready()
	{
		h = maxHealth;
		bar = GetNode<StaminaHealthBar>("StaminaHealthBar");
		bar.Visible = false;
	}

	public override void _Process(double delta)
	{
		if (visibilityTimer.ElapsedMilliseconds >= 4 * 1000)
		{
			bar.Visible = false;
			visibilityTimer.Reset();
		}
		
		if (isRegenerating)
		{
			Health++;
			if (Health >= maxHealth)
			{
				visibilityTimer.Restart();
				isRegenerating = false;
			}
			
			return;
		}
		
		var regenBufferFinished = regenBufferTimer.ElapsedMilliseconds >= regenBufferSeconds * 1000;
		if (regenBufferFinished)
		{
			regenBufferTimer.Reset();
			TriggerRegen();
		}
	}

	public void RemoveHealth(double toRemove)
	{
		Health -= toRemove;
		
		if (Health <= 0)
		{
			Health = 0;
		}
		
		isRegenerating = false;
		regenBufferTimer.Restart();
	}

	public void SetHealth(double newHealth)
	{
		Health = newHealth;
	}

	public void TriggerRegen()
	{
		isRegenerating = true;
	}

	public bool AllowAttack => !(h <= 1);
}
