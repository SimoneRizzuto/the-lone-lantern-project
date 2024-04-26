using Godot;
using Godot.Collections;
using System;

public partial class SceneSwitcher : Node
{
    private Dictionary doorDictionary = new Godot.Collections.Dictionary
    {
        {"RightDoor", "StartPositionLeft"},
        {"LeftDoor", "StartPositionRight"},
        {"HouseDoor", "StartPosition"}
    };

    

    public void HandleSceneSwitch(PackedScene NewScene, string DoorName)
    {

        // Get the starting position based on the doorname
        var startPositionString = doorDictionary[DoorName];
        if (startPositionString.AsStringName() == "")
        {
            GD.PrintErr("DoorName from DoorwayArea does not match any corresponding key in doorDictionary.");
        }
        else
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
                var startPosition = GetNode<Node2D>("./" + nextSceneName.Name + "/GameContainer/" + startPositionString);
                player.Position = startPosition.Position;
            }
            catch
            {

            }
        }


        
        


    }

}
