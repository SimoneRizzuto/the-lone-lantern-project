using Godot;
using System;

public partial class SicilianHouse : Node
{
    public bool entered = false;

    public void OnSicilianHouseTransitionBodyEntered(Node body) // Change input later
    {
        entered = true;
    }

    public void OnSicilianHouseTransitionBodyExited(Node body)
    {
        entered = false;
    }

    public override void _Process(double delta)
    {
        if (entered)
        {
            if (Input.IsActionJustPressed("Enter"))
            {
                GetTree().ChangeSceneToFile("res://Areas//TestSicilianHouse.tscn"); // hard code for testing
            }
        }
    }
}
