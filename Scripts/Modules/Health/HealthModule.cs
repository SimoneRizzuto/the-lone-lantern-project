using Godot;

namespace TheLoneLanternProject.Scripts.Modules.Health;

[GlobalClass]
public partial class HealthModule : Node
{
	[Export] public double MaxHealth = 10.0;
	private double health;
	public override void _Ready()
	{
		health = MaxHealth;
	}
	public void Damage(double value)
	{
		health -= value;
		if (health <= 0)
		{
			GetParent().QueueFree();
		}
	}
}