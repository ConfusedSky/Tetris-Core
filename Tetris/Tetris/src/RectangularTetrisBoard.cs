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
		public RectangularTetrisBoard( int width, int height, int killHeight ) : base()
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
        /// Reset the background of the tetris board
        /// </summary>
        public override void ResetBackground()
        {
            foreach (Block[] row in blocks)
                foreach (Block b in row)
                    b.BackgroundColor = null;
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

		protected override void collapse( int row )
		{	
			ClearRow( row );
			Block[] temp = blocks[ row ];

			for( int i = row; i >= 1; i-- ) {
				blocks[ i ] = blocks[ i - 1 ];
			}

			blocks[ 0 ] = temp;
		}

        protected override void reverseCollapse()
        {
            ClearRow(0);
            Block[] temp = blocks[0];

            for (int i = 1; i < Height; i++)
            {
                blocks[i-1] = blocks[i];
            }

            blocks[Height - 1] = temp;
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
            return blocks[row].All(block => block.Occupied);
		}

		private void ClearRow( int row )
		{
			Block[] r = blocks[row];

			foreach( Block b in r )
				b.Clear();
		}

        /// <summary>
        /// Creates a new board based on the info in text.
        /// </summary>
        /// <param name="text">
        /// An array of strings, strings do not have to all be the same length but must 
        /// be greater than or equal to the length of the first string.
        /// Each string is a row of the tetris board
        /// Anywhere there is an X a black block is placed, other characters are ignored
        /// </param>
        /// <param name="killHeight">
        /// Specifies the kill hight of the board and adds a number of empty rows the the top equal to 
        /// killHeight+1 to prevent death
        /// </param>
        /// <returns>The rectangular tetris board specified in text</returns>
        public static RectangularTetrisBoard CreateBoardFromText( string[] text, int killHeight )
        {
            if (text.Length < 1) return null;
            RectangularTetrisBoard board = new RectangularTetrisBoard(text[0].Length, text.Length+killHeight+1, killHeight);
            char blockToken = 'X';
            Point p = new Point();
            for( p.y = killHeight+1; p.y < board.Height; p.y++ )
            {
                for( p.x = 0; p.x < board.Width; p.x++ )
                {
                    if( text[p.y-killHeight-1][p.x] == blockToken )
                        board.PlaceBlock(p, BlockColor.black);
                }
            }
            return board;
        }
	}
}

