using Godot;
using TheLoneLanternProject.Scripts.Utils.Signals;

namespace TheLoneLanternProject.Scenes.Modules.Combat;

public partial class SpawnerModule : Marker2D
{
    [Export] public PackedScene EnemyPackedScene { get; set; }
    [Export] public string Id;
    
    private CustomSignals customSignals;

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.Spawn += Spawn;
    }

    private void Spawn(EnemySpawnDTO dto)
    {
        var enemy = dto.EnemyScene;
        var id = dto.EnemySceneName;
        if ((enemy != null) || (id != null))
        {
            var enemyScene = enemy.Instantiate();
            enemyScene.Name = id;
            AddChild(enemyScene);
            
        }
        else
        {
            var enemyScene = EnemyPackedScene.Instantiate();
            enemyScene.Name = Id;
            AddChild(enemyScene);
        }
        
        

    }
}