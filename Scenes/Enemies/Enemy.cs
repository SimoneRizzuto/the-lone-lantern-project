using Godot;
using System;
using System.Linq;
using TheLoneLanternProject;
using TheLoneLanternProject.Constants;

public partial class Enemy : EnemyCore
{
    // SIGNALS
    public void OnAreaEntered(Node2D area)
    {
        if (area.IsInGroup(NodeGroup.Attack))
        {
            TakeDamage(1);
        }
    }
}
