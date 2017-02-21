using System.Collections.Generic;

namespace Tetris
{
	public class Mino
	{
		public static BlockColor ShadowColor = BlockColor.white;

		private BaseTetrisBoard board;
		private MinoType type;
		private Point position;
		private int shadowY = -1;
		private int rotation = 0;
		private bool alive = true;

		public bool Alive{ get { return alive; } }
        public Point Position { get { return position; } }

		public MinoType BlockType { 
			get { return type; }
			set { 
				Clear();
				type = value;
				// Move the peice to its proper starting position
				position = CalculateStartingPosition( board, type );
				rotation = 0;
				Place();
				alive = true;
			}
		}

		private Mino( BaseTetrisBoard board, MinoType t, Point startingPosition )
		{
			this.board = board;
			type = t;
			position = startingPosition;
			Place();

			board.BoardChanged += OnBoardChanged;
		}

		// Kill the currently active block
		public void Destroy()
		{
			board.BoardChanged -= OnBoardChanged;
			Clear();
			alive = false;
		}

		// Make sure that when the mino goes out of scope it doesn't continue receiving events from things
		~Mino()
		{
			Destroy();
		}

		public Mino Update( TetrisAction action = TetrisAction.None )
		{
			// If there is no action don't check for action
			if( action == TetrisAction.None ) {
			} else if( action == TetrisAction.Left ) {
				Move( -1, 0 );
			} else if( action == TetrisAction.Right ) {
				Move( 1, 0 );
			} else if( action == TetrisAction.Down ) {
				Move( 0, 1 );
			} else if( action == TetrisAction.Drop ) {
				Move( 0, shadowY - position.y );
				Place( true );
				// Remove any background that would be here
				Clear();
				alive = false;
			} else if( action == TetrisAction.RotateRight ) {
				Rotate( 1 );
			} else if( action == TetrisAction.RotateLeft ) {
				Rotate( 3 );
			}

			return this;
		}

		// If the board is changed, make sure to change the drop location and the shadow as well
		void OnBoardChanged( object sender, System.EventArgs e )
		{
			Clear();
			Place();
		}

		public bool IsValidPlacement( Point offset, int rotationOffset = 0 )
		{
			return board.IsValidPlacement( GetBlockLocations( type, position + offset, (rotation + rotationOffset) % 4 ) );
		}

		public bool Move( Point offset )
		{
			bool result = false;
			Clear();
			if( IsValidPlacement( offset ) ) {
				position += offset;
				result = true;
			}
			Place();
			return result;
		}

		public bool Move( int xOffset, int yOffset )
		{
			return Move( new Point( xOffset, yOffset ) );
		}

        public bool MoveTo( Point p )
        {
            Point offset = p - position;
            return Move(offset);
        }

		// Int parameter so it is possible to do a 180 turn if needed
		// Offset of 1 is a right rotation
		// Offset of 2 is a 180 rotation
		// Offset of 3 is a left rotation
		public bool Rotate( int rotationOffset )
		{
			Clear();
			// try to find a valid offset in which this rotation works
			bool works = false;
			foreach( Point offset in type.GetKickOffsets( rotation, (rotation+rotationOffset)%4 ) ) {
				if( IsValidPlacement( offset, rotationOffset ) ) {
					position += offset;
					works = true;
					break;
				}
			}

			if( !works ) {
				rotationOffset = 0;
			}

			rotation += rotationOffset;
			rotation %= 4;
			Place();
			return true;
		}

		private void SetBlockColor( BlockColor color, bool permanent = false )
		{
			board.PlaceBlocks( GetBlockLocations(), color, background: !permanent );
		}

		public void SetShadowLocation()
		{
			shadowY = position.y;
			// Find the actual place to cast the shadow
			while( board.IsValidPlacement( GetBlockLocations( type, new Point( position.x, shadowY ), rotation ) ) )
				shadowY++;
			shadowY--;
			if( !board.IsValidPlacement( GetBlockLocations( type, new Point( position.x, shadowY ), rotation ) ) )
				shadowY = -1;
		}

		public void SetShadowColor( BlockColor color )
		{
			if( shadowY >= 0 ) {
				board.PlaceBlocks( GetBlockLocations( type, new Point( position.x, shadowY ), rotation ), color, background: true );
			}
		}

		public void Place( bool permanent = false )
		{
			SetShadowLocation();
			SetShadowColor( ShadowColor );
			SetBlockColor( type.BlockColor, permanent );
		}

		public void Clear()
		{
			SetShadowColor( null );
			shadowY = -1;
			SetBlockColor( null );
		}

		public IEnumerable<Point> GetBlockLocations()
		{
			return GetBlockLocations( type, position, rotation );
		}

		public IEnumerable<Block> GetBlocks()
		{
			return board.GetBlocks( GetBlockLocations( type, position, rotation ) );
		}

		// Calculates where a mino of type t would land on the passed in board
		public static Point CalculateStartingPosition( BaseTetrisBoard board, MinoType t )
		{
			Point spawn = board.SpawnPoint;
			if( !board.IsValidPlacement( GetBlockLocations( t, spawn ) ) )
				spawn.y--; 
			return spawn;
		}

		// Creates a new Tetronimo at the center of the screen if the block type can't be placed in the center of the screen return null
		public static Mino CreateNewMino( BaseTetrisBoard board, MinoType t )
		{
			return new Mino( board, t, CalculateStartingPosition( board, t ) );
		}

		public static IEnumerable<Point> GetBlockLocations( MinoType t, Point center, int rotation = 0 )
		{
			foreach( Point offsets in t.GetBlockOffsets(rotation) ) {
				yield return center + offsets;
			}
		}
	}

}


