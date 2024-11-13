using TheLoneLanternProject.Scenes.Player;

namespace TheLoneLanternProject.Modules;

[GlobalClass]
public partial class EnemyWaitingModule : Node
{
    [Export] public EnemyStateMachine State;

    //lambda expression which evaluates if state was waiting
    private bool StateIsValid => State.EnemyState != EnemyState.Waiting;
    
    public override void _Ready()
    {
        // Get state of the parent
        State ?? GetParent<EnemyStateMachine>();
    }

    public override void _Process()
    {
        if (StateIsValid)
        {
            return;
        }
        // set the animation to waiting in the last direction if the state is waiting
        State.MainSprite.Animation = $"waiting {Enum.GetName(State.LastDirection)?.ToLower()}";
    }
}
