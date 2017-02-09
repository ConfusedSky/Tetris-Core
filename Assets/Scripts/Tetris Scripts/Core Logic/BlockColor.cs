using System;

namespace Tetris
{
	/// <summary>
	/// Block color is an immutable struct containing bytes representing a color with each rgba being a number from 0 to 255
	/// </summary>
	public class BlockColor
	{
		public byte r { get; private set; }

		public byte g { get; private set; }

		public byte b { get; private set; }

		public byte a { get; private set; }

		public BlockColor (byte r, byte g, byte b, byte a = 255)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
	}
}