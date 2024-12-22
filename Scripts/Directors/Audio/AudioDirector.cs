using System.Threading.Tasks;
using Godot;
using TheLoneLanternProject.Scripts.Directors.Cutscene;

namespace TheLoneLanternProject.Scripts.Directors.Audio;
public partial class AudioDirector : Node
{
    public static AudioDirector Instance { get; private set; }

    private AsyncActionToPlay asyncActionToPlay = AsyncActionToPlay.NoAction;
    private Task ActionCompleted => actionGiven.Task;
    private TaskCompletionSource actionGiven = new();

    public override void _Ready()
    {
        Instance = this;
    }

    public void PlaySound(string name)
    {
        /*var audioNodes = GetTree().GetNodesInGroup(bus);
    int arraySize = audioNodes.Count;
    if (arraySize == 0)
    {*/
        var newAudioStreamerNode = new AudioStreamPlayer2D();
        newAudioStreamerNode.Stream = (AudioStream)ResourceLoader.Load($"res://Assets/Audio/Sound/{name}.ogg");
        newAudioStreamerNode.Bus = "SFX";
        newAudioStreamerNode.Name = name;
        AddChild(newAudioStreamerNode);
        newAudioStreamerNode.AddToGroup("SFX");
        newAudioStreamerNode.AddToGroup("Audio");
        newAudioStreamerNode.Play();
        return;

        /*}

    foreach (var audioNode in audioNodes)
    {
        if (audioNode.Name == name)
        {
            var existingAudioStreamerNode = (AudioStreamPlayer2D)audioNode;
            existingAudioStreamerNode.Play();
            return;
        }

    }*/
    }

    public void PlayMusic(string name)
    {
        /*var audioNodes = GetTree().GetNodesInGroup(bus);
    int arraySize = audioNodes.Count;
    if (arraySize == 0)
    {*/
        var newAudioStreamerNode = new AudioStreamPlayer2D();
        newAudioStreamerNode.Stream = (AudioStream)ResourceLoader.Load($"res://Assets/Audio/Music/{name}.ogg");
        newAudioStreamerNode.Bus = "Music";
        newAudioStreamerNode.Name = name;
        AddChild(newAudioStreamerNode);
        newAudioStreamerNode.AddToGroup("Music");
        newAudioStreamerNode.AddToGroup("Audio");
        newAudioStreamerNode.Play();
        return;

        /*}

    foreach (var audioNode in audioNodes)
    {
        if (audioNode.Name == name)
        {
            var existingAudioStreamerNode = (AudioStreamPlayer2D)audioNode;
            existingAudioStreamerNode.Play();
            return;
        }

    }*/
    }

    public async Task PlayAudioAsync(string name, string bus)
    {
        asyncActionToPlay = AsyncActionToPlay.NoAction;


        var audioNodes = GetTree().GetNodesInGroup(bus);
        int arraySize = audioNodes.Count;
        if (arraySize == 0)
        {
            var newAudioStreamerNode = new AudioStreamPlayer2D();
            newAudioStreamerNode.Stream = (AudioStream)ResourceLoader.Load($"res://Assets/Audio/{bus}/{name}.ogg");
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


    public void StopAudio(string name, string bus)
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

}