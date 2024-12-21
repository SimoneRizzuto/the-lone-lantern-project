using Godot;
using TheLoneLanternProject.Scripts.Modules.Health;

namespace TheLoneLanternProject.Scripts.Modules.HitBox;

[GlobalClass]
public partial class HitBoxModule : Area2D
{
	[Export] public HealthModule HealthModule;
	public void Damage(double value)
	{
		HealthModule.Damage(value);
	}
}