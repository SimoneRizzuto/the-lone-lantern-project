using Godot;

public partial class Rain : GpuParticles2D
{
	[Export] public bool Raining;
	[Export] public Color Color = new("#9ab2bf");

	public Color InitialColor = new("#ffffff");
	private CanvasModulate canvasModulate;
	
	public override void _Ready()
	{
		AmountRatio = 0;
		canvasModulate = GetNode<CanvasModulate>("CanvasModulate");
		canvasModulate.Color = InitialColor;
	}
	
	public override void _Process(double delta)
	{
		if (Raining)
		{
			AmountRatio += AmountRatio < 1 ? 0.2f * (float)delta : 0;
			
			var tween = GetTree().CreateTween();
			tween.TweenProperty(canvasModulate, "color", Color, 8);
		}
		else if (!Raining)
		{
			AmountRatio -= AmountRatio > 0 ? 0.2f * (float)delta : 0;
			
			var tween = GetTree().CreateTween();
			tween.TweenProperty(canvasModulate, "color", InitialColor, 8);
		}
		
		if (Input.IsActionJustPressed("DebugButton1"))
		{
			Emitting = true;
			Raining = !Raining;
		}
	}
	
	public void TriggerRain()
	{
		Emitting = true;
		Raining = true;
	}
	
	public void StopRain()
	{
		Emitting = true;
		Raining = false;
	}
}
