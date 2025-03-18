using Godot;

namespace TheLoneLanternProject.Scenes.Modules.Combat;
public partial class EnemySpawnDTO : GodotObject
{
    public PackedScene EnemyScene { get; set; }
    public string EnemySceneName { get; set; }
}