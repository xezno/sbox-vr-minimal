using System;

namespace MyGame;

/// <summary>
/// The Hands enum can be used to determine which hand is being used for input.
/// It's a flag so that you can use it to check for both hands at once if you
/// want - for example, if you were making an interaction system where you want
/// both hands to be able to snap to an object.
/// </summary>
[Flags]
public enum Hands
{
	None = 0b0000,
	Left = 0b0001,
	Right = 0b0010,

	Both = Left | Right
}

