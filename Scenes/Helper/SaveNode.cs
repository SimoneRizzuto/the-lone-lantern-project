using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

public partial class SaveNode : Node2D
{
    private Player player;
    public override void _Ready()
    {
        var tree = GetTree();
        var playerNodes = tree.GetNodesInGroup(NodeGroup.Player);
        player = playerNodes.Cast<Player>().FirstOrDefault();
        
        
    }

    public override void _Process(double delta)
    {
        SaveGame();
        LoadGame();
    }

    public Godot.Collections.Dictionary<string, Variant> Serialize(Node saveNode)
    {
        
        if (saveNode.Name == "SceneSwitcher")
        {
            // Assuming that current scene is first child
            var currentScene = saveNode.GetChild(0);
            return new Godot.Collections.Dictionary<string, Variant>()
            {
                {"CurrentSceneName",  currentScene.Name.ToString()},
                {"CurrentSceneProjectPath",  currentScene.SceneFilePath.ToString()},
                {"CurrentSceneTreePath",  currentScene.GetPath().ToString()},
            };
        }
        if (saveNode.Name == "Player")
        {
            var playerNode = (Player)saveNode;


            return new Godot.Collections.Dictionary<string, Variant>()
            {
                {"Health", playerNode.Health},
                {"PositionX", playerNode.Position.X},
                {"PositionY", playerNode.Position.Y},
                {"Direction", playerNode.Direction.ToString()},

            };

        }

        return new Godot.Collections.Dictionary<string, Variant>()
        {
            { saveNode.Name.ToString(), JsonSerializer.Serialize(this)}
        }; // For now try to just save everything

    }

    public void SaveGame()
    {
        if (Input.IsActionPressed(InputMapAction.Save))
        {
            using var saveGame = FileAccess.Open("res://SaveData//SaveData.json", FileAccess.ModeFlags.Write);
            //SaveHelper.SaveHelper.Save(); //Come back to this later.
            var tree = GetTree();
            var saveableNodes = tree.GetNodesInGroup(NodeGroup.Persist);
            var saveJsonString = new Godot.Collections.Dictionary<string, Variant>() { };
            foreach (Node saveNode in saveableNodes)
            {
                var saveDictionary = Serialize(saveNode);
                saveJsonString.Add(saveNode.Name.ToString(), saveDictionary);


            }
            saveGame.StoreLine(Json.Stringify(saveJsonString));

        }

    }

    public void LoadGame()
    {
        if (!FileAccess.FileExists("res://SaveData//SaveData.json"))
        {
            return;
        }

        if (Input.IsActionPressed(InputMapAction.Load))
        {
            using var saveGame = FileAccess.Open("res://SaveData//SaveData.json", FileAccess.ModeFlags.Read);
            var jsonString = saveGame.GetLine();


            var json = new Json();
            var parseResult = json.Parse(jsonString);
            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                return;
            }

            // For now just overwrite the Health value for IO testing
            var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);

            // Current Scene
            var sceneNodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)nodeData["SceneSwitcher"]);
            // Must remove the scene that is child of SceneSwitcher and add the scene from save data.
            //GD.Print(sceneNodeData);
            //var treePath = (string)sceneNodeData["CurrentSceneTreePath"];
            //var sceneToRemove = GetNode<Node>(treePath);
            //RemoveChild(sceneToRemove);

            var projectPath = (string)sceneNodeData["CurrentSceneProjectPath"];
            var nextScenePackedScene = (PackedScene)ResourceLoader.Load(projectPath);
            if (nextScenePackedScene == null)
            {
                GD.PrintErr("Unable to Load. Can't instantiate current scene.");
                return;
            }

            var sceneToAdd = nextScenePackedScene.Instantiate();
            AddChild(sceneToAdd);


            // Player
            var playerNodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)nodeData["Player"]);
            player.Health = (float)playerNodeData["Health"];
            player.Position = new Vector2((float)playerNodeData["PositionX"], (float)playerNodeData["PositionY"]);
            Enum.TryParse((string)playerNodeData["Direction"], out Direction direction);
            player.Direction = direction;


        }


    }
}
