using UnityEngine;

// Defines all of the minos from the original tetris game
public static class Tetromino
{
	public static readonly MinoType I = 
		new MinoType( 
			Offsets:    new Point[]{ new Point( -4, 0 ), new Point( -2, 0 ), new Point( 0, 0 ), new Point( 2, 0 ) },
			BlockColor: Color.cyan,
			Center:     new Point( -1, 1 ),
			Scale:      2
		);

	public static readonly MinoType O =
		new MinoType(
			Offsets:    new Point[]{ new Point( 0, 0 ), new Point( 0, 2 ), new Point( 2, 0 ), new Point( 2, 2 ) },
			BlockColor: Color.yellow,
			Center:     new Point( 1, 1 ),
			Scale:      2
		);

	public static readonly MinoType T = 
		new MinoType(
			Offsets:    new Point[]{ new Point( 0, 0 ), new Point( 0, -1 ), new Point( -1, 0 ), new Point( 1, 0 ) },
			BlockColor: Color.magenta
		);

	public static readonly MinoType J = 
		new MinoType(
			Offsets:    new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 1, 0 ), new Point( -1, -1 ) },
			BlockColor: Color.blue
		);

	public static readonly MinoType L = 
		new MinoType(
			Offsets:    new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 1, 0 ), new Point( 1, -1 ) },
			BlockColor: new Color32( 255, 127, 0, 255 )
		);

	public static readonly MinoType S = 
		new MinoType(
			Offsets:    new Point[]{ new Point( 0, 0 ), new Point( 1, -1 ), new Point( 0, -1 ), new Point( -1, 0 ) },
			BlockColor: Color.green
		);

	public static readonly MinoType Z = 
		new MinoType(
			Offsets:    new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 0, 1 ), new Point( 1, 1 ) },
			BlockColor: Color.red
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
