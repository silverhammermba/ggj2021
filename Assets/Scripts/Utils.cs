using System;
using UnityEngine;

[Flags]
public enum Layers
{
	None = 0,
    Default = 1 << 0,
    TransparentFX = 1 << 1,
    IgnoreRaycast = 1 << 2,
    Water = 1 << 4,
	UI = 1 << 5,
	World = 1 << 8,
}

public class Angle
{
	private Angle() {}

	public static float Degrees(float radians)
	{
		return radians * 180.0f / Mathf.PI;
	}

	public static float Radians(float degrees)
	{
		return degrees * Mathf.PI / 180.0f;
	}

	public static float Degrees(Vector2 direction)
	{
		return Degrees(Radians(direction));
	}

	public static float Radians(Vector2 direction)
	{
		return Mathf.Atan2(direction.y, direction.x);
	}

	public static float DegreeDiff(float start, float end)
	{
		return Degrees(RadianDiff(Radians(start), Radians(end)));
	}

	public static float RadianDiff(float start, float end)
	{
		return Mathf.Atan2(Mathf.Sin(end-start), Mathf.Cos(end-start));
	}
}
