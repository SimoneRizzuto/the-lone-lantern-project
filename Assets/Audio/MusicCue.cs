using Godot;
using TheLoneLanternProject.Constants;

public partial class MusicCue : Node2D
{
    public AudioDirector audioDirector;

    public override void _Ready()
    {
        audioDirector = AudioDirector.Instance;
    }

    public void OnBackgroundMusicBodyEntered(Node2D area)
    {
        if (area.IsInGroup(NodeGroup.Player)) {
            audioDirector.PlayMusic("Grasslands Theme"); 
        }

            
    }
}
