using Godot;

namespace TheLoneLanternProject.Modules;

[GlobalClass]
public partial class HitBoxModule : Area2D
{
	[Export] public HealthModule HealthModule;
	public void Damage(double value)
	{
		HealthModule.Damage(value);
	}
}