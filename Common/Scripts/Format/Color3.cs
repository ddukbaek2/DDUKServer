using System; // IComparable, IConvertible, IEquatable<T>, IFormattable


namespace DDUKServer
{
	public struct Color3<T> where T : struct, IComparable, IConvertible, IEquatable<T>, IFormattable
	{
		public static Color3<T> Identity = new Color3<T>
		{
			R = default,
			G = default,
			B = default,
		};

		public T R { set; get; } = default;
		public T G { set; get; } = default;
		public T B { set; get; } = default;

		public Color3()
		{
		}
	}
}