using UnityEngine;
using System.Collections.Generic;

public class Mino
{
	public static Color ShadowColor = Color.white;

	private TetrisBoard board;
	private MinoType type;
	private Point position;
	private int shadowY = -1;
	private int rotation = 0;
	private bool alive = true;

	public bool Alive{ get { return alive; } }

	public MinoType BlockType { 
		get { return type; }
		set 
		{ 
			Clear();
			type = value;
			// Move the peice to its proper starting position
			position = CalculateStartingPosition( board, type );
			rotation = 0;
			Place();
			alive = true;
		}
	}

	// Define a table containing tetronimos represented as an array of offsets
	// +y is down +x is right
	public static Point[][] TETROMINO = new Point[7][] { 
		// I
		new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( -2, 0 ), new Point( 1, 0 ) },
		// O
		new Point[]{ new Point( 0, 0 ), new Point( 1, 0 ), new Point( 0, 1 ), new Point( 1, 1 ) },
		// T
		new Point[]{ new Point( 0, 0 ), new Point( 0, -1 ), new Point( -1, 0 ), new Point( 1, 0 ) },
		// J
		new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 1, 0 ), new Point( -1, -1 ) },
		// L
		new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 1, 0 ), new Point( 1, -1 ) },
		// S
		new Point[]{ new Point( 0, 0 ), new Point( 1, -1 ), new Point( 0, -1 ), new Point( -1, 0 ) },
		// Z
		new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 0, 1 ), new Point( 1, 1 ) }
	};

	public static Color[] TETROMINO_COLORS = new Color[7] {
		// I
		Color.cyan,
		// O
		Color.yellow,
		// T
		Color.magenta,
		// J
		Color.blue,
		// L
		new Color32( 255, 127, 0, 255 ),
		// S
		Color.green,
		// Z
		Color.red
	};

	public static MinoType[] TETROMINO_TYPES = new MinoType[]
	{
		// I
		new MinoType( TETROMINO[0], TETROMINO_COLORS[0] ),
		// O
		new MinoType( TETROMINO[1], TETROMINO_COLORS[1] ),
		// T
		new MinoType( TETROMINO[2], TETROMINO_COLORS[2] ),
		// J
		new MinoType( TETROMINO[3], TETROMINO_COLORS[3] ),
		// L
		new MinoType( TETROMINO[4], TETROMINO_COLORS[4] ),
		// S
		new MinoType( TETROMINO[5], TETROMINO_COLORS[5] ),
		// Z
		new MinoType( TETROMINO[6], TETROMINO_COLORS[6] )
	};

	public static int TETROMINIO_COUNT
	{
		get
		{
			return TETROMINO_TYPES.Length;
		}
	}

	public Mino(TetrisBoard board, MinoType t, Point startingPosition)
	{
		this.board = board;
		type = t;
		position = startingPosition;
		Place();

		board.OnBoardChanged += OnBoardChanged;
	}

	// Kill the currently active block
	public void Destroy()
	{
		Clear();
		board.OnBoardChanged -= OnBoardChanged;
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
		if( action == TetrisAction.None )
		{
		}
		else if( action == TetrisAction.Left )
		{
			Move( -1, 0 );
		}
		else if( action == TetrisAction.Right )
		{
			Move( 1, 0 );
		}
		else if( action == TetrisAction.Down )
		{
			Move( 0, 1 );
		}
		else if( action == TetrisAction.Drop )
		{
			Move( 0, shadowY - position.y );
			Place( true );
			// Remove any background that would be here
			Clear();
			alive = false;
		}
		else if( action == TetrisAction.RotateRight )
		{
			Rotate();
		}

		return this;
	}

	// If the board is changed, make sure to change the drop location and the shadow as well
	void OnBoardChanged (object sender, System.EventArgs e)
	{
		Clear();
		Place();
	}

	public bool ValidPlacement( Point offset,  int rotationOffset = 0 )
	{
		return board.ValidPlacement( GetBlockLocations( type, position + offset, (rotation + rotationOffset)%4) );
	}

	public bool Move( Point offset )
	{
		bool result = false;
		Clear();
		if( ValidPlacement( offset ) )
		{
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

	public bool Rotate()
	{
		int rotationOffset = 1;
		Clear();
		while( rotationOffset != 0 && !ValidPlacement( Point.Origin, rotationOffset ) )
		{
			// try to find a valid offset in which this rotation works
			bool works = false;
			foreach( Point offset in new Point[]{ new Point(  0,  1 ),
				                                   new Point(  1,  0 ), 
				                                   new Point( -1,  0 ), 
				                                   new Point(  0, -1 ) } )
			{
				if( ValidPlacement( offset, rotationOffset ) )
				{
					position += offset;
					works = true;
					break;
				}
			}

			if( !works )
			{
				rotationOffset++;
				rotationOffset %= 4;
			}
		}
		rotation += rotationOffset;
		rotation %= 4;
		Place();
		return true;
	}

	private void SetBlockColor( Color? color, bool permanent = false )
	{
		board.PlaceBlocks( GetBlockLocations(), color, background: !permanent );
	}

	public void SetShadowLocation()
	{
		shadowY = position.y;
		// Find the actual place to cast the shadow
		while( shadowY < board.Height && board.ValidPlacement( GetBlockLocations( type, new Point( position.x, shadowY ), rotation ) ) ) shadowY++;
		shadowY--;
		if( !board.ValidPlacement( GetBlockLocations( type, new Point( position.x, shadowY ), rotation ) ) ) shadowY = -1;
	}

	public void SetShadowColor( Color? color )
	{
		if( shadowY >= 0 && shadowY < board.Height )
		{
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

	public static IEnumerable<Point> GetBlockLocations( MinoType t, Point center, int rotation = 0 )
	{
		foreach( Point offsets in t.GetBlockOffsets(rotation) )
		{
			yield return center + offsets;
		}
	}

	public IEnumerable<TetrisBlockScript> GetBlocks()
	{
		return board.GetBlocks( GetBlockLocations( type, position, rotation ) );
	}

	public static IEnumerable<TetrisBlockScript> GetBlocks( TetrisBoard board, MinoType t, Point position, int rotation = 0 )
	{
		return board.GetBlocks( GetBlockLocations( t, position, rotation ) );
	}

	public static bool ValidPlacement( TetrisBoard board, MinoType t, Point position, int rotation = 0 )
	{
		return board.ValidPlacement( GetBlockLocations( t, position, rotation ) );
	}
		
	public static Mino CreateRandomTetronimo( TetrisBoard board )
	{
		int t = (int)Random.Range( 0, TETROMINIO_COUNT );
		return CreateNewMino( board, TETROMINO_TYPES[t] );
	}

	// Calculates where a mino of type t would land on the passed in board
	public static Point CalculateStartingPosition( TetrisBoard board, MinoType t )
	{
		return new Point( board.Width / 2, board.ValidPlacement( GetBlockLocations( t, new Point( board.Width / 2, 0 ) ) ) ? 0 : -1 );
	}

	// Creates a new Tetronimo at the center of the screen if the block type can't be placed in the center of the screen return null
	public static Mino CreateNewMino( TetrisBoard board, MinoType t )
	{
		return new Mino( board, t, CalculateStartingPosition( board, t ) );
	}
}


