using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
	public class RectangularTetrisBoard : BaseTetrisBoard
	{
		private Block[][] blocks;

		public int Width { get; private set; }

		public int Height { get; private set; }

		public int KillHeight { get; private set; }

		public override Point SpawnPoint {
			get {
				return new Point( Width/2, 0 );
			}
		}

		public override bool Lost {
			get {
				for( int i = 0; i < Width; i++ )
				{
					if( blocks[KillHeight][i].Occupied )
						return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Tetris.RectangularTetrisBoard"/> class.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		/// <param name="killHeight">Kill height.</param>
		public RectangularTetrisBoard( int width, int height, int killHeight )
		{
			Width = width;
			Height = height;
			KillHeight = killHeight;
			blocks = new Block[Height][];

			for( int i = 0; i < Height; i++ ) {
				blocks[i] = new Block[Width];

				for( int j = 0; j < Width; j++ ) {
					blocks[i][j] = new Block();
				}
			}
		}

		/// <summary>
		/// Reset the internal state of the tetris board
		/// </summary>
		public override void Reset()
		{
			foreach( Block[] row in blocks )
				foreach( Block b in row )
					b.Clear();
		}

		/// <summary>
		/// Determines whether the specified point is a valid placement.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="point">Point to check.</param>
		public override bool IsValidPlacement( Point point )
		{
			if( point.x < 0 || point.x >= Width || point.y >= Height ||
			    (point.y >= 0 && GetBlockAt( point ).Occupied) ) {
				return false;
			} else {
				return true;
			}
		}

		public override Block GetBlockAt( Point p )
		{
			if( p.y >= 0 )
				return blocks[p.y][p.x];
			else
				return null;
		}

		public override void CollapseRow( int row )
		{	
			ClearRow( row );
			Block[] temp = blocks[ row ];

			for( int i = row; i >= 1; i-- ) {
				blocks[ i ] = blocks[ i - 1 ];
			}

			blocks[ 0 ] = temp;
		}

		public override IList<int> CheckClears()
		{
			IEnumerable<int> clears = from row in Enumerable.Range( 0, Height )
			                          where CheckClear( row )
			                          select row;
			return new List<int>( clears );
		}

		public override bool CheckClear( int row )
		{
			Block[] r = blocks[row];

			foreach( Block b in r ) {
				if( !b.Occupied ) {
					return false;
				}
			}

			return true;
		}

		private void ClearRow( int row )
		{
			Block[] r = blocks[row];

			foreach( Block b in r )
				b.Clear();
		}
	}
}

