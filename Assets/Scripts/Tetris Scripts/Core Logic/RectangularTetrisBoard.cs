using System;
using System.Collections.Generic;

namespace Tetris
{
	public class RectangularTetrisBoard : BaseTetrisBoard
	{
		private Block[][] blocks;

		public int Width { get; private set; }
		public int Height { get; private set; }
		public int Blocks { get; }

		public RectangularTetrisBoard ( int width, int height, int killHeight )
		{
			Width = width;
			Height = height;
			blocks = new Block[Height][];

			for (int i = 0; i < Height; i++) {
				blocks [i] = new Block[Width];

				for( int j = 0; j < Width; j++ ) {
					blocks [i] [j] = new Block ();
				}
			}
		}

		/// <summary>
		/// Reset the internal state of the tetris board
		/// </summary>
		public override void Reset ()
		{
			foreach (Block[] row in blocks)
				foreach (Block b in row)
					b.Clear ();
		}

		/// <summary>
		/// Determines whether the specified point is a valid placement.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="point">Point to check.</param>
		public override bool IsValidPlacement (Point point)
		{
			if( point.x < 0 || point.x >= Width || point.y >= Height ||
				(point.y >= 0 && GetBlockAt(point).Occupied ) )
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public override Block GetBlockAt (Point p)
		{
			return blocks [p.y] [p.x];
		}
	}
}

