using Godot;
using System;
using TheLoneLanternProject.Extensions;
using TheLoneLanternProject.Modules;
using TheLoneLanternProject.Scenes.Player;

public partial class Luce3 : CharacterBody2D
{
	[Export] public Vector2 CalculatedVelocity;
	
	private CustomSignals customSignals = new();
	private PlayerStateMachine playerStateMachine;
	
	public override void _Ready()
	{
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		playerStateMachine ??= GetNode<PlayerStateMachine>(nameof(PlayerStateMachine));
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Velocity = CalculatedVelocity * (float)delta;
		MoveAndSlide();
		
		var roundedPosition = Position.RoundToNearestValue(0.25f);
		Position = roundedPosition;
		
		if (Input.IsActionJustPressed("TestTriggerDialogue")) // TEST DIALOGUE TRIGGER FOR NOW, DELETE LATER
		{
			customSignals.EmitSignal(nameof(CustomSignals.ShowDialogueBalloon), "dialogue-test", "initial_dialogue");
		}
	}
	
	public void SetState(PlayerState state)
	{
		playerStateMachine.PlayerState = state;
	}
}
