using System;
using System.Collections.Generic;

namespace Tetris
{
	public class RowCollapseEventArgs : System.EventArgs
	{
		public IList<int> ClearedRows{ get; private set; }

		public RowCollapseEventArgs( IList<int> clearedRows )
		{
			ClearedRows = clearedRows;
		}
	}

	/// <summary>
	/// Base class for tetris board implementations
	/// Can not be instantiated
	/// </summary>
	public abstract class BaseTetrisBoard
	{
		public BaseTetrisBoard()
		{
			BoardChanged += BaseTetrisBoard_BoardChanged;
			RowCollapsed += BaseTetrisBoard_RowCollapsed;
		}

		~BaseTetrisBoard()
		{
			BoardChanged -= BaseTetrisBoard_BoardChanged;
			RowCollapsed -= BaseTetrisBoard_RowCollapsed;
		}

		/// <summary>
		/// Occurs when the board is changed changed.
		/// </summary>
		public event System.EventHandler BoardChanged;

		private void BaseTetrisBoard_BoardChanged (object sender, EventArgs e)
		{
			CollapseAll();
		}

		/// <summary>
		/// Occurs when a row is collapsed.
		/// </summary>
		public event System.EventHandler<RowCollapseEventArgs> RowCollapsed;

		private void BaseTetrisBoard_RowCollapsed (object sender, RowCollapseEventArgs e)
		{
            BoardChanged?.Invoke(this, EventArgs.Empty);
        }

		/// <summary>
		/// Default location to spawn things onto the board
		/// </summary>
		/// <value>The spawn point.</value>
		public abstract Point SpawnPoint { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="Tetris.BaseTetrisBoard"/> is in a loss state.
		/// </summary>
		/// <value><c>true</c> if lost; otherwise, <c>false</c>.</value>
		public abstract bool Lost{ get; }

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

			if (!background)
                BoardChanged?.Invoke(this, EventArgs.Empty);
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

			if (!background && BoardChanged != null)
				BoardChanged( this, EventArgs.Empty );
		}

		/// <summary>
		/// Place a block at point p, whose color is color.
		/// </summary>
		/// <param name="p">Point to place the block.</param>
		/// <param name="color">Color of the block.</param>
		private void place (Point p, BlockColor color, bool background)
		{
			Block b = GetBlockAt (p);

			if (b == null)
				return;

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
				Block b = GetBlockAt (p);
				if( b != null )
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

		protected abstract void collapse( int row );

		/// <summary>
		/// Collapses a row of the board.
		/// </summary>
		/// <param name="row">Row to be collapsed</param>
		public void CollapseRow( int row )
		{
			collapse( row );
            RowCollapsed?.Invoke(this, new RowCollapseEventArgs(new System.Collections.Generic.List<int>(row)));
        }

		/// <summary>
		/// Collapses all Clearable Rows.
		/// </summary>
		/// <returns>The cleared rows</returns>
		public IList<int> CollapseAll()
		{
			IList<int> clears = CheckClears();
			// if the previous frame had a drop that cleared resolve the clear
			foreach( int i in clears )
			{
				collapse( i );
			}
			if( clears.Count > 0 && RowCollapsed != null ) RowCollapsed( this, new RowCollapseEventArgs( clears ) );

			return clears;
		}

		/// <summary>
		/// Checks all the rows of a tetris board for clearable rows
		/// </summary>
		/// <returns>Returns a list of the row numbers that are clearable</returns>
		public abstract IList<int> CheckClears();

		/// <summary>
		/// Checks if a single row can be cleared
		/// </summary>
		/// <returns><c>true</c>, if row was clearable, <c>false</c> otherwise.</returns>
		/// <param name="row">Row to check</param>
		public abstract bool CheckClear( int row );

	}

}
