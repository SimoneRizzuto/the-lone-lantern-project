using Godot;

public partial class SceneSwitcher : Node
{
    public void HandleSceneSwitch(PackedScene NewScene)
    {
        // Assuming that the scene switcher node is the top and the current scene the player is in is the first child node
        var currentSceneName = GetChild(0);

        var nextSceneName = NewScene.Instantiate();
        RemoveChild(currentSceneName);
        AddChild(nextSceneName);

        // Set the player to the startPosition if there is one
        var player = GetNode<Node2D>("./" + nextSceneName.Name + "/GameContainer/PlayerController");
        try
        {
            var startPosition = GetNode<Node2D>("./" + nextSceneName.Name + "/GameContainer/StartPosition");
            player.Position = startPosition.Position;
        }
        catch { 
            
        }
        


    }

}
