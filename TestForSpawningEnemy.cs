using Godot;
using System;

public partial class TestForSpawningEnemy : Node2D
{
    [Export]
    public PackedScene EnemyScene { get; set; }

    public override void _Ready()
    {
        GetNode<Timer>("./EnemyTimer").Start(GD.RandRange(0, 1));
    }

    public void OnEnemyTimerTimeout()
    {
        var enemy = EnemyScene.Instantiate<Enemy>();
        AddChild(enemy);
        GetNode<Timer>("./EnemyTimer").Start(GD.RandRange(20, 30));
        enemy.PlayerTarget = GetNode<CharacterBody2D>("./PlayerController/Player");
    }
}
