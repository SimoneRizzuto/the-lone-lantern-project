using Godot;
using System;
using System.Linq;
using System.Text.Json;
using TheLoneLanternProject.Constants;
using TheLoneLanternProject.Scenes.Player;

public partial class SaveNode : Node2D
{
    private Luce3 luce;
    public override void _Ready()
    {
        var tree = GetTree();
        var playerNodes = tree.GetNodesInGroup(NodeGroup.Player);
        luce = playerNodes.Cast<Luce3>().FirstOrDefault();

    }

    public override void _Process(double delta)
    {
        SaveGame();
        LoadGame();
    }

    public Godot.Collections.Dictionary<string, Variant> Serialize(Node saveNode)
    {
        // Scene
        if (saveNode.Name == "SceneSwitcher")
        {
            var currentScene = saveNode.GetChild(0); // Assuming that current scene is first child

            return new Godot.Collections.Dictionary<string, Variant>()
            {
                {"CurrentSceneName",  currentScene.Name.ToString()},
                {"CurrentSceneProjectPath",  currentScene.SceneFilePath.ToString()},
                {"CurrentSceneTreePath",  currentScene.GetPath().ToString()},
            };
        }

        // Player
        if (saveNode.Name == "Player")
        {
            var playerNode = (Luce3)saveNode;


            return new Godot.Collections.Dictionary<string, Variant>()
            {
                //{"Health", playerNode.Health},
                {"PositionX", playerNode.Position.X},
                {"PositionY", playerNode.Position.Y},
                //{"Direction", playerNode.Direction.ToString()},

            };

        }

        // Other
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            { saveNode.Name.ToString(), JsonSerializer.Serialize(this)}
        }; 

    }

    public void SaveGame()
    {
        if (Input.IsActionPressed(InputMapAction.Save))
        {
            // Write to savefile
            using var saveGame = FileAccess.Open("res://SaveData//SaveData.json", FileAccess.ModeFlags.Write);
            
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
            // Read in savefile
            using var saveGame = FileAccess.Open("res://SaveData//SaveData.json", FileAccess.ModeFlags.Read);

            var jsonString = saveGame.GetLine();
            var json = new Json();
            var parseResult = json.Parse(jsonString);

            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                return;
            }

            var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
            var sceneNodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)nodeData["SceneSwitcher"]);

            // Scene
            var sceneSwitcher = GetNode<Node>("/root/Main/SceneSwitcher");
            var currentScene = sceneSwitcher.GetChild(0);
            currentScene.Free();

            var projectPath = (string)sceneNodeData["CurrentSceneProjectPath"];
            var nextScenePackedScene = (PackedScene)ResourceLoader.Load(projectPath);
            if (nextScenePackedScene == null)
            {
                GD.PrintErr("Unable to Load. Can't instantiate current scene.");
                return;
            }

            var sceneToAdd = nextScenePackedScene.Instantiate();
            sceneSwitcher.AddChild(sceneToAdd); 


            // Player
            var tree = GetTree();
            var playerNodes = tree.GetNodesInGroup(NodeGroup.Player);
            luce = playerNodes.Cast<Luce3>().FirstOrDefault();

            var playerNodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)nodeData["Player"]);
            //luce.Health = (float)playerNodeData["Health"];
            luce.Position = new Vector2((float)playerNodeData["PositionX"], (float)playerNodeData["PositionY"]);
            Enum.TryParse((string)playerNodeData["Direction"], out Direction direction);
            //luce.Direction = direction;


        }


    }
}
