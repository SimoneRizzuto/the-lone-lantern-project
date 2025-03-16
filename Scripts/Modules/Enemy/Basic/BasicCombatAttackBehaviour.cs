using Godot;
using System;
using System.Linq;
using TheLoneLanternProject.Scripts.Shared.Constants;

namespace TheLoneLanternProject.Scripts.Modules.Enemy.Basic;

[GlobalClass]
public partial class BasicCombatAttackBehaviour : BaseEnemyBehaviour
{
    public override void _PhysicsProcess(double delta)
    {
        if (StateMachine.EnemyState is not EnemyState.CombatAttack)
        {
            return;
        }

        var attack = Attacks.FirstOrDefault();
        Console.WriteLine(attack?.AttackName);
        Console.WriteLine(attack?.AnimationName);
        Attacks.FirstOrDefault()?.TriggerAttack();
        
    }
    
    // attack objects can probably be static in here.
    // each attack needs to be assignable for this Behavior to find it.
    
    // Because each Behaviour is custom, instancing the attacks statically is probably fine?
    // experiment with two attacks in here
    // consider: animations, hitboxes, length of attack, what state the enemy will go into
    // all of this makes me think each shouldn't be a static object, but instead a PackedScene???
    // and each packed scene gets added as a child or some other way??????
}