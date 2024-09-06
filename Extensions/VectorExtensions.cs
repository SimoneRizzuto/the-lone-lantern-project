using Godot;

namespace TheLoneLanternProject.Helpers;

public static class VectorExtensions
{
    public static Vector2 Round(this Vector2 vector, int decimalPlaces = 1)
    {
        float factor = Mathf.Pow(10, decimalPlaces);
        return new Vector2(Mathf.Round(vector.X * factor) / factor, Mathf.Round(vector.Y * factor) / factor);
    }
}