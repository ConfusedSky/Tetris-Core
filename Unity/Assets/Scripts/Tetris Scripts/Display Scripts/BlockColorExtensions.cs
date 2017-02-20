using UnityEngine;

public static class BlockColorExtensions
{
	// Explicit conversion from BlockColor to Color32
	public static Color32? ToUnityColor( this Tetris.BlockColor c )
	{
		if( c == null )
			return null;
		else
			return new Color32( c.r, c.g, c.b, c.a );
	}

	public static Tetris.BlockColor ToBlockColor( this Color32 c )
	{
		return new Tetris.BlockColor( c.r, c.g, c.b, c.a );
	}

	public static Tetris.BlockColor ToBlockColor( this Color c )
	{
		return ToBlockColor( (Color32)c );
	}
		
}
