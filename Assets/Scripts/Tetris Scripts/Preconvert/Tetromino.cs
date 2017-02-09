using UnityEngine;
using Tetris;

// Defines all of the minos from the original tetris game
public static class Tetromino
{
	// General kicks, used for most of the blocks
	public static readonly Point[][] GeneralKickOffsetData = new Point[][] 
	{
		new Point[]{ new Point( 0, 0 ), new Point( 0, 0 ), new Point( 0, 0 ), new Point( 0, 0 ), new Point( 0, 0 ) },
		new Point[]{ new Point( 0, 0 ), new Point( 1, 0 ), new Point( 1, 1 ), new Point( 0,-2 ), new Point( 1,-2 ) },
		new Point[]{ new Point( 0, 0 ), new Point( 0, 0 ), new Point( 0, 0 ), new Point( 0, 0 ), new Point( 0, 0 ) },
		new Point[]{ new Point( 0, 0 ), new Point(-1, 0 ), new Point(-1, 1 ), new Point( 0,-2 ), new Point(-1,-2 ) }
	};

	// General kicks, used for most of the blocks
	public static readonly Point[][] IKickOffsetData = new Point[][] 
	{
		new Point[]{ new Point( 0, 0 ), new Point(-1, 0 ), new Point( 2, 0 ), new Point(-1, 0 ), new Point( 2, 0 ) },
		new Point[]{ new Point(-1, 0 ), new Point( 0, 0 ), new Point( 0, 0 ), new Point( 0,-1 ), new Point( 0, 2 ) },
		new Point[]{ new Point(-1,-1 ), new Point( 1,-1 ), new Point(-2,-1 ), new Point( 1, 0 ), new Point(-2, 0 ) },
		new Point[]{ new Point( 0,-1 ), new Point( 0,-1 ), new Point( 0,-1 ), new Point( 0, 1 ), new Point( 0,-2 ) }
	};
	
	public static readonly MinoType I = 
		new MinoType( 
			Offsets:     new Point[]{ new Point( -1, 0 ), new Point( 0, 0 ), new Point( 1, 0 ), new Point( 2, 0 ) },
			KickOffsets: IKickOffsetData,
			BlockColor:  ((Color32)Color.cyan).ToBlockColor()
		);

	public static readonly MinoType O =
		new MinoType(
			Offsets:     new Point[]{ new Point( 0, 0 ), new Point( 0, -1 ), new Point( 1, 0 ), new Point( 1, -1 ) },
			KickOffsets: new Point[][]
		                 { 
			             	new Point[]{ new Point(  0, 0 ) }, 
			                new Point[]{ new Point(  0, 1 ) }, 
			                new Point[]{ new Point( -1, 1 ) }, 
			                new Point[]{ new Point( -1, 0 ) } 
		                 },
			BlockColor:  ((Color32)Color.yellow).ToBlockColor()
		);

	public static readonly MinoType T = 
		new MinoType(
			Offsets:     new Point[]{ new Point( 0, 0 ), new Point( 0, -1 ), new Point( -1, 0 ), new Point( 1, 0 ) },
			KickOffsets: GeneralKickOffsetData,
			BlockColor:  ((Color32)Color.magenta).ToBlockColor()
		);

	public static readonly MinoType J = 
		new MinoType(
			Offsets:     new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 1, 0 ), new Point( -1, -1 ) },
			KickOffsets: GeneralKickOffsetData,
			BlockColor: ((Color32)Color.blue).ToBlockColor()
		);

	public static readonly MinoType L = 
		new MinoType(
			Offsets:     new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 1, 0 ), new Point( 1, -1 ) },
			KickOffsets: GeneralKickOffsetData,
			BlockColor:  new BlockColor( 255, 127, 0, 255 )
		);

	public static readonly MinoType S = 
		new MinoType(
			Offsets:     new Point[]{ new Point( 0, 0 ), new Point( 1, -1 ), new Point( 0, -1 ), new Point( -1, 0 ) },
			KickOffsets: GeneralKickOffsetData,
			BlockColor:  ((Color32)Color.green).ToBlockColor()
		);

	public static readonly MinoType Z = 
		new MinoType(
			Offsets:     new Point[]{ new Point( 0, 0 ), new Point( -1, 0 ), new Point( 0, 1 ), new Point( 1, 1 ) },
			KickOffsets: GeneralKickOffsetData,
			BlockColor:  ((Color32)Color.red).ToBlockColor()
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
