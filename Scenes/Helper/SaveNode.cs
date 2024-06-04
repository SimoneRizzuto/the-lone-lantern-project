using Godot;
using Newtonsoft.Json;
using System.Linq;
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

    public static Godot.Collections.Dictionary<string, Variant> Save()
    {
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            {"CurrentScene","2"},
            //{"Health": },
        };
    }

    public void SaveGame()
    {
        if (Input.IsActionPressed(InputMapAction.Save))
        {
            using var saveGame = FileAccess.Open("res://SaveData//SaveData.json", FileAccess.ModeFlags.Write);
            //SaveHelper.SaveHelper.Save(); //Come back to this later.

            var saveDictionary = new Godot.Collections.Dictionary<string, Variant>()
            {
                {"Player", JsonConvert.SerializeObject(this)},
                {"Health", player.Health},
                // For now while testing the IO system assume that scene won't change
            };

            var jsonString = Json.Stringify(saveDictionary);
            GD.Print(saveDictionary);


            saveGame.StoreLine(jsonString);

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
            player.Health = (float)nodeData["Health"];

        }


    }
}
