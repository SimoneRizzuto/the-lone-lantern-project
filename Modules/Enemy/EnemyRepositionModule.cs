using TheLoneLanternProject.Helpers;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Modules;

[GlobalClass]
public partial class EnemyRepositionModule : Node
{
    [Export] public EnemyStateMachine EnemyState;
    [Export] public Vector2 MovementVector;
    [Export] public float MoveSpeed = DefaultMoveSpeed; // Set as needed

    public static readonly float DefaultMoveSpeed = 4000;
    public static readonly int MoveVelocityThreshold = 25; // might change

    private float MovementVectorThreshold => MoveVelocityThreshold / 100f;
    private bool StateIsValid => State.EnemyState is EnemyState.Attacking;

    public override void _Ready()
    {
        State ?? GetParent<EnemyStateMachine>();
    }
}
