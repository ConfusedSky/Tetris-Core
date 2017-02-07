using UnityEngine;

// Defines all of the minos from the original tetris game
public static class Tetromino
{
	public static readonly MinoType I = 
		new MinoType( 
			new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 1, 0 ), new Point( 2, 0 ) },
			Color.cyan
			//new Point( 1, 0 ),
			//2
		);

	public static readonly MinoType O =
		new MinoType(
			new Point[]{ new Point( 0, 0 ), new Point( 0, 2 ), new Point( 2, 0 ), new Point( 2, 2 ) },
			Color.yellow,
			new Point( 1, 1 ),
			2
		);

	public static readonly MinoType T = 
		new MinoType(
			new Point[]{ new Point( 0, 0 ), new Point( 0, -1 ), new Point( -1, 0 ), new Point( 1, 0 ) },
			Color.magenta
		);

	public static readonly MinoType J = 
		new MinoType(
			new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 1, 0 ), new Point( -1, -1 ) },
			Color.blue
		);

	public static readonly MinoType L = 
		new MinoType(
			new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 1, 0 ), new Point( 1, -1 ) },
			new Color32( 255, 127, 0, 255 )
		);

	public static readonly MinoType S = 
		new MinoType(
			new Point[]{ new Point( 0, 0 ), new Point( 1, -1 ), new Point( 0, -1 ), new Point( -1, 0 ) },
			Color.green
		);

	public static readonly MinoType Z = 
		new MinoType(
			new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 0, 1 ), new Point( 1, 1 ) },
			Color.red
		);

	public static readonly MinoType[] TETROMINO_TYPES = new MinoType[]
	{
		I, O, T, J, L, S, Z
	};
		
	public static Mino CreateRandomTetronimo( TetrisBoard board )
	{
		int t = Random.Range( 0, TETROMINO_TYPES.Length );
		return Mino.CreateNewMino( board, TETROMINO_TYPES[t] );
	}
}
