using Godot;
using System;

namespace TheLoneLanternProject.Scripts.Shared.Extensions;
public static class VectorExtensions
{
    public static Vector2 Round(this Vector2 vector, int decimalPlaces = 1)
    {
        var factor = Mathf.Pow(10, decimalPlaces);
        return new Vector2(Mathf.Round(vector.X * factor) / factor, Mathf.Round(vector.Y * factor) / factor);
    }
    
    public static Vector2 RoundToNearestValue(this Vector2 vector2, double nearestValue = 1, MidpointRounding mode = MidpointRounding.ToEven)
    {
        var xPosition = Math.Round(vector2.X / nearestValue, mode) * nearestValue;
        var yPosition = Math.Round(vector2.Y / nearestValue, mode) * nearestValue;
        return new Vector2((float)xPosition, (float)yPosition);
    }
    
    public static Vector2 GetRandomDirection(double min, double max)
    {
        var x = GetRandomNumber(min, max);
        var y = GetRandomNumber(min, max);
        return new Vector2(x, y);
    }
    
    private static float GetRandomNumber(double minimum, double maximum)
    { 
        var random = new Random();
        var value = random.NextDouble() * (maximum - minimum) + minimum;
        return (float)value;
    }
}