using System;

public class Point
{
	public int x = 0;
	public int y = 0;

	public Point( int x, int y )
	{
		this.x = x;
		this.y = y;
	}

	public static Point operator+( Point first, Point other )
	{
		return new Point( first.x + other.x, first.y + other.y );
	}
}

