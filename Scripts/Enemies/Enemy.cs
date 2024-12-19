using Godot;
using TheLoneLanternProject.Scripts.Extensions;

namespace TheLoneLanternProject.Scripts.Enemies;

public partial class Enemy : CharacterBody2D
{
    [Export] public Vector2 CalculatedVelocity;
    public override void _PhysicsProcess(double delta)
    {
        Velocity = CalculatedVelocity * (float)delta;
        MoveAndSlide();

        var roundedPosition = Position.RoundToNearestValue(0.25f);
        Position = roundedPosition;
    }
}