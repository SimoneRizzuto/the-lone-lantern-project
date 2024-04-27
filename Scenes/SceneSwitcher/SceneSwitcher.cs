using Godot;
using Godot.Collections;
using System;

public partial class SceneSwitcher : Node
{
    private CustomSignals customSignals = new CustomSignals(); 
    
    private Dictionary doorDictionary = new Godot.Collections.Dictionary
    {
        {"RightDoor", "StartPositionLeft"},
        {"LeftDoor", "StartPositionRight"},
        {"HouseDoor", "StartPosition"}
    };

    public override void _Ready()
    {
        customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        customSignals.SceneSwitch += HandleSceneSwitch;
    }

    private void HandleSceneSwitch(PackedScene NewScene, string DoorName)
    {

        // Get the starting position based on the doorname
        var startPositionNodeString = doorDictionary[DoorName];
        if (startPositionNodeString.AsStringName() == "")
        {
            GD.PrintErr("DoorName from DoorwayArea does not match any corresponding key in doorDictionary.");
        }
        else
        {
            var currentSceneName = GetChild(0);
            var nextSceneName = NewScene.Instantiate();
            RemoveChild(currentSceneName);
            AddChild(nextSceneName);
            
            var player = GetNode<Node2D>("./" + nextSceneName.Name + "/GameContainer/PlayerController");
            try
            {
                var newStartPosition = GetNode<Node2D>("./" + nextSceneName.Name + "/GameContainer/" + startPositionNodeString);
                player.Position = newStartPosition.Position;
            }
            catch
            {

            }
        }

    }

}
