using System;
using System.Collections.Generic;

namespace Tetris
{
	/// <summary>
	/// Base tetris board.
	/// </summary>
	public abstract class BaseTetrisBoard
	{
		/// <summary>
		/// Occurs when the board is changed changed.
		/// </summary>
		public event System.EventHandler BoardChanged;

		/// <summary>
		/// Reset the internal state of the tetris board
		/// </summary>
		public abstract void Reset ();

		/// <summary>
		/// Determines whether the specified BlockLocations are a valid placement in this instance.
		/// </summary>
		/// <returns><c>true</c> if all of the blocks sent in are empty and in the boundries; otherwise, <c>false</c>.</returns>
		/// <param name="BlockLocations">Block locations to test.</param>
		public bool IsValidPlacement (IEnumerable<Point> BlockLocations)
		{
			foreach (Point point in BlockLocations) {
				if (!IsValidPlacement (point))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Determines whether this instance is valid placement the specified point.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid placement the specified point; otherwise, <c>false</c>.</returns>
		/// <param name="point">Point to test.</param>
		public abstract bool IsValidPlacement (Point point);

		/// <summary>
		/// Places the blocks at the specified block locations.
		/// </summary>
		/// <param name="BlockLocations">Block locations to place blocks at.</param>
		/// <param name="color">Color.</param>
		/// <param name="background">If set to <c>true</c> Blocks are placed in the background rather than the forground.</param>
		public void PlaceBlocks (IEnumerable<Point> BlockLocations, BlockColor color, bool background = false)
		{
			foreach (Point p in BlockLocations) {
				place (p, color, background);
			}

			if (BoardChanged != null && !background)
				BoardChanged (this, System.EventArgs.Empty);
		}

		/// <summary>
		/// Places the block at point p.
		/// </summary>
		/// <param name="p">P.</param>
		/// <param name="color">Color of the block.</param>
		/// <param name="background">If set to <c>true</c> Block is placed in the background rather than the forground.</param>
		public void PlaceBlock (Point p, BlockColor color, bool background = false)
		{
			place (p, color, background);

			if (BoardChanged != null && !background)
				BoardChanged (this, System.EventArgs.Empty);
		}

		/// <summary>
		/// Place a block at point p, whose color is color.
		/// </summary>
		/// <param name="p">Point to place the block.</param>
		/// <param name="color">Color of the block.</param>
		private void place (Point p, BlockColor color, bool background)
		{
			Block b = GetBlockAt (p);

			if (background)
				b.BackgroundColor = color;
			else
				b.Color = color;
		}

		/// <summary>
		/// Gets the block scripts at the specified block locations
		/// </summary>
		/// <returns>The blocks.</returns>
		/// <param name="BlockLocations">Block locations to get blocks from.</param>
		public IEnumerable<Block> GetBlocks (IEnumerable<Point> BlockLocations)
		{
			foreach (Point p in BlockLocations) {
				yield return GetBlockAt (p);
			}
		}

		/// <summary>
		/// Gets the block at point p.
		/// </summary>
		/// <returns>The <see cref="Tetris.Block"/>.</returns>
		/// <param name="p">Location of the block to return</param>
		public abstract Block GetBlockAt (Point p);

		/// <summary>
		/// Gets the block at the specified point.
		/// </summary>
		/// <param name="p">Specified point</param>
		public Block this[ Point p ]
		{
			get { return GetBlockAt (p); }
		}
	}

}
