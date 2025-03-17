using Godot;
using TheLoneLanternProject.Scripts.Utils.Signals;

namespace TheLoneLanternProject.Scenes.Modules.Combat;

public partial class SpawnerModule : Marker2D
{
    [Export] public PackedScene EnemyUid { get; set; }
    [Export] public string Id;
    
    private CustomSignals customSignals;

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.Spawn += Spawn;
    }

    private void Spawn() //PackedScene? enemy, string? id
    {
        /*if (enemy == null)
        {
            var enemyScene = enemy.Instantiate();
            enemyScene.Name = id;
            AddChild(enemyScene);
            
        }*/
        //else
        //{
        
        var enemyScene = EnemyUid.Instantiate();
        enemyScene.Name = Id;
        AddChild(enemyScene);
        //}
        
        

    }
}