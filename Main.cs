using Godot;

public partial class Main : Node
{
    [Export]
    public PackedScene EnemyScene { get; set; }

    public override void _Ready()
    {
        GetNode<Timer>("./EnemyTimer").Start(GD.RandRange(0, 1));
    }

    public void OnEnemyTimerTimeout()
    {
        Enemy enemy = EnemyScene.Instantiate<Enemy>();
        AddChild(enemy);
        GetNode<Timer>("./EnemyTimer").Start(GD.RandRange(20, 30));
    }


}
