using Godot;
using System.Threading.Tasks;

public partial class AudioDirector: Node
{
    public static AudioDirector Instance { get; private set;}

    private AsyncActionToPlay asyncActionToPlay = AsyncActionToPlay.NoAction;
    private Task ActionCompleted => actionGiven.Task;
    private TaskCompletionSource actionGiven = new();

    public override void _Ready()
    {
        Instance = this;
        //TestMusicBeforeCutscene();
    }

    public void PlaySound(string name, string bus)
    {
        var audioNodes = GetTree().GetNodesInGroup(bus);
        int arraySize = audioNodes.Count;
        if (arraySize == 0)
        {
            var newAudioStreamerNode = new AudioStreamPlayer2D();
            newAudioStreamerNode.Stream = (AudioStream)ResourceLoader.Load($"res://Assets/Audio/Scratch/{name}.ogg");
            newAudioStreamerNode.Bus = bus;
            newAudioStreamerNode.Name = name;
            AddChild(newAudioStreamerNode);
            newAudioStreamerNode.AddToGroup(bus);
            newAudioStreamerNode.AddToGroup("Audio");
            newAudioStreamerNode.Play();
            return;

        }

        foreach (var audioNode in audioNodes)
        {
            if (audioNode.Name == name)
            {
                var existingAudioStreamerNode = (AudioStreamPlayer2D)audioNode;
                existingAudioStreamerNode.Play();
                return;
            }

        }
    }

    public async Task PlaySoundAsync(string name, string bus)
    {
        asyncActionToPlay = AsyncActionToPlay.NoAction;


        var audioNodes = GetTree().GetNodesInGroup(bus);
        int arraySize = audioNodes.Count;
        if (arraySize == 0)
        {
            var newAudioStreamerNode = new AudioStreamPlayer2D();
            newAudioStreamerNode.Stream = (AudioStream)ResourceLoader.Load($"res://Assets/Audio/Scratch/{name}.ogg");
            newAudioStreamerNode.Bus = bus;
            newAudioStreamerNode.Name = name;
            AddChild(newAudioStreamerNode);
            newAudioStreamerNode.AddToGroup(bus);
            newAudioStreamerNode.AddToGroup("Audio");
            newAudioStreamerNode.Play();
            await ToSignal(newAudioStreamerNode, "finished");
            return;

        }

        foreach (var audioNode in audioNodes)
        {
            if (audioNode.Name == name)
            {
                var existingAudioStreamerNode = (AudioStreamPlayer2D)audioNode;
                existingAudioStreamerNode.Play();
                await ToSignal(existingAudioStreamerNode, "finished");
                return;
            }
        }
    }


    public void StopSound(string name, string bus)
    {
        var audioNodes = GetTree().GetNodesInGroup(bus);
        foreach (var audioNode in audioNodes)
        {
            if (audioNode.Name == name)
            {
                RemoveChild(audioNode);
                audioNode.QueueFree();
                return;
            }

        }
        GD.Print("Could not find specified sound to stop.");
        return;
    }

    public void StopAllAudio()
    {
        // Remove all nodes from scene in the Audio node group
        var audioNodes = GetTree().GetNodesInGroup("Audio");
        foreach (var audioNode in audioNodes)
        {
            var streamingPlayer = (AudioStreamPlayer2D)audioNode;
            streamingPlayer.VolumeDb = 0;
            streamingPlayer.Stop();
            streamingPlayer.EmitSignal("finished");
            RemoveChild(audioNode);
            audioNode.QueueFree();

        }
        return;
    }



    public void TestMusicBeforeCutscene()
    {
        PlaySound("Intro Theme", "Music");
        
    }
}
