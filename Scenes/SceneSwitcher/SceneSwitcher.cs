using Godot;
using System;

public partial class SceneSwitcher : Node
{
    public void HandleSceneSwitch(string currentLevelName)
    {
        string nextSceneName;
        GD.Print(currentLevelName);

        if (currentLevelName == "TestOutsideArea")
        {
            nextSceneName = "TestSicilianHouse";
        }
        else if (currentLevelName == "CouchArea")
        {
            nextSceneName = "Couch";
        }
        else
        {
            return;
        }

        // Assuming folder areas holds all areas in game. 
        GetTree().ChangeSceneToFile("res://Areas/" + nextSceneName + ".tscn");
    }

}
