using UnityEngine;

[System.Serializable]
public enum TetronimoType : int
{
	I = 0,
	O = 1,
	T = 2,
	J = 3,
	L = 4,
	S = 5,
	Z = 6
}

public class Tetronimo
{
	public Color ShadowColor = Color.white;

	private TetrisBlockScript[,] space;
	private TetronimoType type;
	private int x;
	private int y;
	private int shadowY = -1;

	// Define a table containing tetronimos represented as an array of offsets
	// +y is down +x is right
	public static readonly int[,,] TETRONIMOS = new int[7, 4, 2] { 
		// I
		{ { 0, 0 }, { -1, 0 }, { -2, 0 }, { 1, 0 } },
		// O
		{ { 0, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } },
		// T
		{ { 0, 0 }, { 0, 1 }, { -1, 1 }, { 1, 1 } },
		// J
		{ { 0, 0 }, { 0, 1 }, { 0, 2 }, { -1, 2 } },
		// L
		{ { 0, 0 }, { 0, 1 }, { 0, 2 }, { 1, 2 } },
		// S
		{ { 0, 0 }, { 1, 0 }, { 0, 1 }, { -1, 1 } },
		// Z
		{ { 0, 0 }, { -1, 0 }, { 0, 1 }, { 1, 1 } }
	};

	public static readonly Color[] TETRONIMO_COLORS = new Color[7] {
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

	public Tetronimo(TetrisBlockScript[,] space, TetronimoType t, int startingX, int startingY)
	{
		this.space = space;
		type = t;
		x = startingX;
		y = startingY;
		Place();
	}

	public void Update( TetrisAction action = TetrisAction.None )
	{
		if( action == TetrisAction.Left )
		{
			Move( -1, 0 );
		}
		else if( action == TetrisAction.Right )
		{
			Move( 1, 0 );
		}
		else if( Input.GetKeyDown( KeyCode.Space ) )
		{
			Clear();
			type = (TetronimoType)(((int)type + 1) % TETRONIMO_COUNT);
			Place();
		}
	}

	public bool ValidPlacement( int xOffset, int yOffset )
	{
		return ValidPlacement( space, type, x + xOffset, y + yOffset );
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
		while( shadowY < space.GetLength(0) && ValidPlacement( space, type, x, shadowY ) ) shadowY++;
		shadowY--;
		if( !ValidPlacement( space, type, x, shadowY ) ) shadowY = -1;
	}

	public void SetShadowColor( Color? color )
	{
		if( shadowY >= 0 && shadowY < space.GetLength(0) )
		{
			foreach( TetrisBlockScript block in GetBlocks( space, type, x, shadowY ) )
				block.BackgroundColor = color;
		}
	}

	public void Place()
	{
		SetShadowLocation();
		SetShadowColor( new Color( .7f, .7f, .7f ) );
		SetBlockColor( TETRONIMO_COLORS[(int)type] );
	}

	public void Clear()
	{
		SetShadowColor( null );
		shadowY = -1;
		SetBlockColor( null );
	}

	public System.Collections.IEnumerable GetBlockLocations()
	{
		return GetBlockLocations( type, x, y );
	}

	public static System.Collections.IEnumerable GetBlockLocations( TetronimoType t, int x, int y )
	{
		for( int i = 0; i < 4; i++ )
		{
			yield return new int[]{ (x + TETRONIMOS[(int)t, i, 0]), (y + TETRONIMOS[(int)t, i, 1]) };
		}
	}

	public System.Collections.IEnumerable GetBlocks()
	{
		return GetBlocks( space, type, x, y );
	}

	public static System.Collections.IEnumerable GetBlocks( TetrisBlockScript[,] space, TetronimoType t, int x, int y )
	{
		foreach( int[] point in GetBlockLocations(t,x,y) )
		{
			yield return space[point[1], point[0]];
		}
	}

	public static bool ValidPlacement( TetrisBlockScript[,] space, TetronimoType t, int x, int y )
	{
		//for( int i = 0; i < 4; i++ )
		//{
		//	int ix = (x + TETRONIMOS[(int)t, i, 0]);
		//	int iy = (y + TETRONIMOS[(int)t, i, 1]);
		//	if( ix < 0 || ix >= space.GetLength( 1 ) ||
		//		iy < 0 || iy >= space.GetLength( 0 ) ||
		//	    space[iy, ix].Occupied )
		//	{
		//		return false;
		//	}
		//}
		foreach( int[] point in GetBlockLocations( t, x, y ) )
		{
			if( point[0] < 0 || point[0] >= space.GetLength( 1 ) ||
			    point[1] < 0 || point[1] >= space.GetLength( 0 ) ||
				space[point[1], point[0]].Occupied )
			{
				return false;
			}
		}
		return true;
	}
		
	public static Tetronimo CreateRandomTetronimo( TetrisBlockScript[,] space )
	{
		TetronimoType t = (TetronimoType)Random.Range(0,TETRONIMO_COUNT);
		return CreateNewTetronimo( space, t );
	}

	// Creates a new Tetronimo at the center of the screen if the block type can't be placed in the center of the screen return null
	public static Tetronimo CreateNewTetronimo( TetrisBlockScript[,] space, TetronimoType t )
	{
		if( !ValidPlacement( space, t, space.GetLength( 1 ) / 2, 0 ) )
		{
			return null;
		}

		return new Tetronimo( space, t, space.GetLength( 1 ) / 2, 0 );
	}
}


