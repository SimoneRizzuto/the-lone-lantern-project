using Godot;

namespace TheLoneLanternProject.Scripts.Modules.Bars.Health;

[GlobalClass]
public partial class HealthModule : Node
{
	[Export] private double maxHealth = 1D;
	public bool IsDead => health <= 0;
	
	private double health;
	public override void _Ready()
	{
		health = maxHealth;
	}
	public void DealDamage(double value = 1D)
	{
		health -= value;
		if (health <= 0)
		{
			GetParent().QueueFree();
		}
	}
}