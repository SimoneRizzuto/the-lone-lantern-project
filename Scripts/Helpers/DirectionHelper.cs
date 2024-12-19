using Godot;
using TheLoneLanternProject.Scripts.Constants;

namespace TheLoneLanternProject.Scripts.Helpers;
public static class DirectionHelper
{
    public static Direction GetSnappedDirection(Vector2 input, float length = 0.0001f)
    {
        // Normalize the input to ensure consistent direction (ignore magnitude)
        if (input.Length() < length) return Direction.None; // No direction if input is zero

        input = input.Normalized();

        // Get the angle in radians (-PI to PI) from the vector (using atan2)
        double angle = Mathf.Atan2(input.Y, input.X);

        // Convert the angle from radians to degrees
        var angleDegrees = angle * 180 / Mathf.Pi;

        // Round the angle to the nearest 45 degrees
        var snappedAngle = Mathf.Round(angleDegrees / 90) * 90;

        switch (snappedAngle)
        {
            // Return corresponding Direction based on snapped angle
            case 0:
            case 360:
                return Direction.Right;
            case 90:
                return Direction.Down;
            case 180:
            case -180:
                return Direction.Left;
            case -90:
            case 270:
                return Direction.Up;
            default:
                return Direction.None;
        }
    }
}