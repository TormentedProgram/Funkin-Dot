using Godot;
using System;

public partial class CoolUtil : Node
{
    public static float formatAccuracy(float input) {
        float roundedValue = Mathf.Round(input * 10000) / 10000f;
        return Mathf.Clamp(roundedValue * 100, 0f, 100f);
    }

    public static float BoundTo(float value, float min, float max)
    {
        return Mathf.Max(min, Mathf.Min(max, value));
    }
}
