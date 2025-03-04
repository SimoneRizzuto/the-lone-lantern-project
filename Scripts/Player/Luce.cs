using System;
using System.Numerics;
using Godot;
using TheLoneLanternProject.Scripts.Enemies;
using TheLoneLanternProject.Scripts.Utils.Signals;
using TheLoneLanternProject.Scripts.Shared.Constants;
using TheLoneLanternProject.Scripts.Shared.Extensions;
using TheLoneLanternProject.Scripts.StateMachines.Player;
using Vector2 = Godot.Vector2;

namespace TheLoneLanternProject.Scripts.Player;
public partial class Luce : CharacterBody2D
{
	[Export] public Vector2 CalculatedVelocity;
	[Export] public Enemy Enemy1;
	[Export] public Enemy Enemy2;
	
	
	private CustomSignals customSignals = new();
	private PlayerStateMachine playerStateMachine;
	
	public override void _Ready()
	{
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		playerStateMachine ??= GetNode<PlayerStateMachine>(nameof(PlayerStateMachine));
	}

	private bool enemy1deleto = false;
	private bool enemy2deleto = false;
	
	public override void _PhysicsProcess(double delta)
	{
		
		
		if (Input.IsActionJustPressed("Delete Enemy 1") && !enemy1deleto)
		{
			Enemy1?.QueueFree();
			enemy1deleto = true;
		}
		if (Input.IsActionJustPressed("Delete Enemy 2") && !enemy2deleto)
		{
			Enemy2?.QueueFree();
			enemy2deleto = true;
		}
		
		
		Velocity = CalculatedVelocity * (float)delta;
		MoveAndSlide();
		
		var roundedPosition = Position.RoundToNearestValue(0.25f);
		Position = roundedPosition;
		
		if (Input.IsActionJustPressed("TestTriggerDialogue")) // TEST DIALOGUE TRIGGER FOR NOW, DELETE LATER
		{
			customSignals.EmitSignal(nameof(CustomSignals.ShowDialogueBalloon), "dialogue-test", "initial_dialogue");
		}
	}
	
	public PlayerStateMachine GetStateMachine()
	{
		return playerStateMachine;
	}
	
	public void SetPlayerState(PlayerState state)
	{
		playerStateMachine.PlayerState = state;
		
		if (state == PlayerState.Disabled && !playerStateMachine.IsInteracting)
		{
			playerStateMachine.MainSprite.Animation = $"idle {Enum.GetName(playerStateMachine.LastDirection)?.ToLower()}";
			CalculatedVelocity = Vector2.Zero;
		}
	}
	public void SetDirection(Direction direction)
	{
		playerStateMachine.LastDirection = direction;
	}

	public void ToggleInteracting(bool? isInteracting = null)
	{
		playerStateMachine.IsInteracting = isInteracting ?? !playerStateMachine.IsInteracting;
	}
}