using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MinoType 
{
	/// Define mino represented as an array of offsets
	// +y is down +x is right
	public Point[] Offsets;
	public Color Color;
}
