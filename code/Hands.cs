using System;

namespace MyGame;

[Flags]
public enum Hands
{
	None = 0b0000,
	Left = 0b0001,
	Right = 0b0010,

	Both = Left | Right
}
