using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MinoType 
{
	/// Define mino represented as an array of offsets
	// +y is down +x is right
	public Point[] Offsets;
	public Color BlockColor;
	public Point Center;

	public MinoType( Point[] Offsets, Color BlockColor )
	{
		this.Offsets = Offsets;
		this.BlockColor = BlockColor;
		this.Center = Point.Origin;
	}

	public MinoType( Point[] Offsets, Color BlockColor, Point Center )
	{
		this.Offsets = Offsets;
		this.BlockColor = BlockColor;
		this.Center = Center;
	}

	public IEnumerable<Point> GetBlockOffsets( int rotation )
	{
		foreach( Point p in Offsets )
		{
			yield return p.Rotate( Center, rotation );
		}
	}
}
