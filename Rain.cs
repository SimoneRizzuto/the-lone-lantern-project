using Godot;

public partial class Rain : GpuParticles2D
{
	[Export] public bool Raining;
	[Export] public CanvasModulate CanvasModulate;
	
	public override void _Ready()
	{
		AmountRatio = 0;
	}
	
	public override void _Process(double delta)
	{
		if (Raining)
		{
			AmountRatio += AmountRatio < 1 ? 0.2f * (float)delta : 0;
		}
		else if (!Raining)
		{
			AmountRatio -= 0.2f * (float)delta;
			CanvasModulate.Modulate
		}
		
		if (Input.IsActionJustPressed("DebugButton1"))
		{
			Emitting = true;
			Raining = !Raining;
		}
		
		// blend the colour as well
	}
	
	public void BeginRaining()
	{
		Emitting = true;
		Raining = true;
		
		AmountRatio = 0;
	}
	
	public void StopRaining()
	{
		Emitting = true;
		Raining = false;
	}
}
