using UnityEngine;
using System.Collections.Generic;

public class Tetronimo
{
	public static Color ShadowColor = Color.white;

	private TetrisBoard board;
	private TetronimoType type;
	private int x;
	private int y;
	private int shadowY = -1;
	private int rotation = 0;

	public TetronimoType BlockType { get { return type; } }

	// Define a table containing tetronimos represented as an array of offsets
	// +y is down +x is right
	public static Point[][] TETRONIMOS = new Point[7][] { 
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

	public static Color[] TETRONIMO_COLORS = new Color[7] {
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

	public static int TETRONIMO_COUNT
	{
		get
		{
			return System.Enum.GetNames( typeof(TetronimoType) ).Length;
		}
	}

	public Tetronimo(TetrisBoard board, TetronimoType t, int startingX, int startingY)
	{
		this.board = board;
		type = t;
		x = startingX;
		y = startingY;
		Place();
	}

	public Tetronimo Update( TetrisAction action = TetrisAction.None )
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
			Move( 0, shadowY - y );
			return null;
		}
		else if( action == TetrisAction.RotateRight )
		{
			Rotate();
		}

		return this;
	}

	public bool ValidPlacement( int xOffset, int yOffset, int rotationOffset = 0 )
	{
		return ValidPlacement( board, type, x + xOffset, y + yOffset, (rotation + rotationOffset)%4 );
	}

	public bool Move( int xOffset, int yOffset )
	{
		bool result = false;
		Clear();
		if( ValidPlacement( xOffset, yOffset ) )
		{
			x += xOffset;
			y += yOffset;
			result = true;
		}
		Place();
		return result;
	}

	public bool Rotate()
	{
		int rotationOffset = 1;
		Clear();
		while( rotationOffset != 0 && !ValidPlacement( 0, 0, rotationOffset ) )
		{
			// try to find a valid offset in which this rotation works
			bool works = false;
			foreach( int[] offsets in new int[][]{ new int[]{  0,  1 },
				                                   new int[]{  1,  0 }, 
				                                   new int[]{ -1,  0 }, 
				                                   new int[]{  0, -1 } } )
			{
				if( ValidPlacement( offsets[0], offsets[1], rotationOffset ) )
				{
					x += offsets[0];
					y += offsets[1];
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

	private void SetBlockColor( Color? c )
	{
		foreach( TetrisBlockScript block in GetBlocks() )
		{
			block.BlockColor = c;
		}
	}

	public void SetShadowLocation()
	{
		shadowY = y;
		// Find the actual place to cast the shadow
		while( shadowY < board.Height && ValidPlacement( board, type, x, shadowY, rotation ) ) shadowY++;
		shadowY--;
		if( !ValidPlacement( board, type, x, shadowY, rotation ) ) shadowY = -1;
	}

	public void SetShadowColor( Color? color )
	{
		if( shadowY >= 0 && shadowY < board.Height )
		{
			foreach( TetrisBlockScript block in GetBlocks( board, type, x, shadowY, rotation ) )
				block.BackgroundColor = color;
		}
	}

	public void Place()
	{
		SetShadowLocation();
		SetShadowColor( ShadowColor );
		SetBlockColor( type.Color() );
	}

	public void Clear()
	{
		SetShadowColor( null );
		shadowY = -1;
		SetBlockColor( null );
	}

	public IEnumerable<Point> GetBlockLocations()
	{
		return GetBlockLocations( type, x, y, rotation );
	}

	public static IEnumerable<Point> GetBlockLocations( TetronimoType t, int x, int y, int rotation = 0 )
	{
		foreach( Point point in TETRONIMOS[(int)t] )
		{
			int[] offsets = RotateOffset( point.x, point.y, rotation );
			yield return new Point(x + offsets[0], y + offsets[1]);
		}
	}

	public IEnumerable<TetrisBlockScript> GetBlocks()
	{
		return GetBlocks( board, type, x, y, rotation );
	}

	public static IEnumerable<TetrisBlockScript> GetBlocks( TetrisBoard board, TetronimoType t, int x, int y, int rotation = 0 )
	{
		return board.GetBlocks( GetBlockLocations( t, x, y, rotation ) );
	}

	public static bool ValidPlacement( TetrisBoard board, TetronimoType t, int x, int y, int rotation = 0 )
	{
		return board.ValidPlacement( GetBlockLocations( t, x, y, rotation ) );
	}

	public static int[] RotateOffset( int xOffset, int yOffset, int rotation )
	{
		int[] result;

		switch( rotation )
		{
		case 0:
			result = new int[]{ xOffset, yOffset };
			break;
		case 1:
			result = new int[]{ -yOffset, xOffset };
			break;
		case 2:
			result = new int[]{ -xOffset, -yOffset };
			break;
		case 3:
			result = new int[]{ yOffset, -xOffset };
			break;
		default:
			result = new int[]{ xOffset, yOffset };
			break;
		}

		return result;
	}
		
	public static Tetronimo CreateRandomTetronimo( TetrisBoard board )
	{
		TetronimoType t = (TetronimoType)Random.Range( 0, TETRONIMO_COUNT );
		return CreateNewTetronimo( board, t );
	}

	// Creates a new Tetronimo at the center of the screen if the block type can't be placed in the center of the screen return null
	public static Tetronimo CreateNewTetronimo( TetrisBoard board, TetronimoType t )
	{
		if( !board.ValidPlacement( GetBlockLocations( t, board.Width / 2, 0 ) ) )
		{
			return new Tetronimo( board, t, board.Width / 2, -1 );
		}

		return new Tetronimo( board, t, board.Width / 2, 0 );
	}
}


