using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MinoType 
{
	/// Define mino represented as an array of offsets
	// +y is down +x is right
	public Point[] Offsets;
	public Color BlockColor;
	// The scale of the offsets being used. This allows off grid centers
	public int Scale;
	public Point Center;

	public MinoType( Point[] Offsets, Color BlockColor, Point Center, int Scale = 1 )
	{
		this.Offsets = Offsets;
		this.BlockColor = BlockColor;
		this.Center = Center;
		this.Scale = Scale;
	}

	public MinoType( Point[] Offsets, Color BlockColor, int Scale = 1 )
	{
		this.Offsets = Offsets;
		this.BlockColor = BlockColor;
		this.Center = Point.Origin;
		this.Scale = Scale;
	}

	public IEnumerable<Point> GetBlockOffsets( int rotation )
	{
		foreach( Point p in Offsets )
		{
			yield return p.Rotate( rotation, Center ) / Scale;
		}
	}
}
