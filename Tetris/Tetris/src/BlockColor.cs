using System;

namespace Tetris
{
	/// <summary>
	/// Block color is an immutable struct containing bytes representing a color with each rgba being a number from 0 to 255
	/// </summary>
	public class BlockColor
	{
		public static BlockColor black   = new BlockColor(   0,   0,   0 );
		public static BlockColor blue    = new BlockColor(   0,   0, 255 );
		public static BlockColor clear   = new BlockColor(   0,   0,   0,  0 );
		public static BlockColor cyan    = new BlockColor(   0, 255, 255 );
		public static BlockColor gray    = new BlockColor( 127, 127, 127 );
		public static BlockColor green   = new BlockColor(   0, 255,   0 );
		public static BlockColor grey    = gray;
		public static BlockColor magenta = new BlockColor( 255,   0, 255 );
		public static BlockColor red     = new BlockColor( 255,   0,   0 );
		public static BlockColor white   = new BlockColor( 255, 255, 255 );
		public static BlockColor yellow  = new BlockColor( 255, 235,   4 );
		public static BlockColor orange  = new BlockColor( 255, 127,   0 );

		
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