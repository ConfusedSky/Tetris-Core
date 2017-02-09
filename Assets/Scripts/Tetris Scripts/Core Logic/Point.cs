using System;

namespace Tetris
{

	[Serializable]
	public class Point
	{
		public static readonly Point Origin = new Point (0, 0);

		public int x = 0;
		public int y = 0;

		public Point (int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public static Point operator+ (Point first, Point other)
		{
			return new Point (first.x + other.x, first.y + other.y);
		}

		public static Point operator- (Point first, Point other)
		{
			return new Point (first.x - other.x, first.y - other.y);
		}

		public static Point operator/ (Point first, int other)
		{
			return new Point (first.x / other, first.y / other);
		}

		// Rotates the point around the origin
		// Rotates by
		// 0, 0 degrees
		// 1, 90 degrees
		// 2, 180 degrees
		// 3, 270 degrees
		public Point Rotate (int rotation)
		{
			Point result;

			switch (rotation) {
			case 0:
				result = new Point (x, y);
				break;
			case 1:
				result = new Point (-y, x);
				break;
			case 2:
				result = new Point (-x, -y);
				break;
			case 3:
				result = new Point (y, -x);
				break;
			default:
				result = new Point (x, y);
				break;
			}

			return result;
		}

		// Rotate around a point
		public Point Rotate (int rotation, Point centerPoint)
		{
			return new Point (x - centerPoint.x, 
				y - centerPoint.y).Rotate (rotation) + centerPoint;
		}

	}

}
