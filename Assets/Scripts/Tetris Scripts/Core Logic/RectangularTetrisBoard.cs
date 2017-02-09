using System;
using System.Collections.Generic;

namespace Tetris
{
	public class RectangularTetrisBoard : BaseTetrisBoard
	{
		private Block[][] blocks;

		public int Width { get; private set; }
		public int Height { get; private set; }

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

		public override void Reset ()
		{
			foreach (Block[] row in blocks)
				foreach (Block b in row)
					b.Clear ();
		}

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

		protected override void place (Point p, BlockColor color, bool background)
		{
			
		}

		public override IEnumerable<Block> GetBlocks (IEnumerable<Point> BlockLocations)
		{
			throw new NotImplementedException ();
		}

		public override Block GetBlockAt (Point p)
		{
			throw new NotImplementedException ();
		}
	}
}

