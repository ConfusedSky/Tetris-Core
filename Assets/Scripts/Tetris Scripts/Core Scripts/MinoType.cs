
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MinoType 
{
	/// Define mino represented as an array of offsets
	/// +y is down +x is right
	private Point[] offsets;
	private Point[][] kicks;
	private Color color;
	/// The scale of the offsets being used. This allows off grid centers
	private int scale;
	private Point center;

	public Color BlockColor { get{ return color; } }

	public MinoType( Point[] Offsets, Point[][] KickOffsets, Color BlockColor, Point Center, int Scale = 1 )
	{
		this.offsets = Offsets;
		this.color = BlockColor;
		this.center = Center;
		this.scale = Scale;
		this.kicks = KickOffsets;
		if( kicks.Length < 4 )
			throw new System.ArgumentException( "KickOffsets must have 4 arrays of offset data!" );
	}

	public MinoType( Point[] Offsets, Point[][] KickOffsets, Color BlockColor, int Scale = 1 ) 
		: this( Offsets, KickOffsets, BlockColor, Point.Origin, Scale ) {}

	public IEnumerable<Point> GetBlockOffsets( int rotation )
	{
		foreach( Point p in offsets )
		{
			yield return p.Rotate( rotation, center ) / scale;
		}
	}

	// Gives the kicks from one rotation to another. DOES NOT CHECK TO MAKE SURE SOURCE/DESTINATION ARE LESS THAN 4
	public IEnumerable<Point> GetKickOffsets( int sourceRotation, int destinationRotation )
	{
		for( int i = 0; i < kicks[sourceRotation].Length && i < kicks[destinationRotation].Length; i++ )
		{
			yield return kicks[sourceRotation][i] - kicks[destinationRotation][i];
		}
	}
}
