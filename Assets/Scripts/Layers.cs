using System;

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
