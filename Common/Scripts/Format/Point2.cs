using System; // IComparable, IConvertible, IEquatable<T>, IFormattable


namespace DDUKServer
{
	public struct Point2<T> where T : struct, IComparable, IConvertible, IEquatable<T>, IFormattable
	{
		public static Point2<T> Identity = new Point2<T>
		{
			X = default,
			Y = default,
		};

		public T X { set; get; } = default;
		public T Y { set; get; } = default;

		public Point2()
		{
		}

		public static Point2<T> operator +(Point2<T> left, Point2<T> right)
		{
			return new Point2<T>
			{
				X = (dynamic)left.X + right.X,
				Y = (dynamic)left.Y + right.Y,
			};
		}

		public static Point2<T> operator -(Point2<T> left, Point2<T> right)
		{
			return new Point2<T>
			{
				X = (dynamic)left.X - right.X,
				Y = (dynamic)left.Y - right.Y,
			};
		}

		public static Point2<T> operator *(Point2<T> left, Point2<T> right)
		{
			return new Point2<T>
			{
				X = (dynamic)left.X * right.X,
				Y = (dynamic)left.Y * right.Y,
			};
		}

		public static Point2<T> operator /(Point2<T> left, Point2<T> right)
		{
			return new Point2<T>
			{
				X = (dynamic)left.X / right.X,
				Y = (dynamic)left.Y / right.Y,
			};
		}
	}
}