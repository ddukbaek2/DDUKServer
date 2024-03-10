using System;


namespace DDUKServer.HTML
{
	/// <summary>
	/// 절대 길이 (픽셀).
	/// </summary>
	public struct AbsoluteLength : ILength
	{
		public double Value { set; get; }

		public AbsoluteLength() => Set(default);
		public AbsoluteLength(double value) => Set(value);
		//public AbsoluteLength(RelativeLength relativeLength) => Set(relativeLength.Value, relativeLength.UnitOfLength);
		public AbsoluteLength(AbsoluteLength absoluteLength) => Set(absoluteLength.Value);

		/// <summary>
		/// 설정.
		/// </summary>
		public void Set(double value)
		{
			Value = value;
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		void ILength.Set(double value, UnitOfLength unitOfLength)
		{
			if (unitOfLength != UnitOfLength.Pixel)
				throw new ArgumentException();

			Set(value);
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		UnitOfLength ILength.GetUnitOfLength() => UnitOfLength.Pixel;

		/// <summary>
		/// 문자열 반환 (인터페이스 구현).
		/// </summary>
		public readonly override string ToString() => LengthHelper.ToString(Value, UnitOfLength.Pixel);

		public static AbsoluteLength Add(AbsoluteLength left, AbsoluteLength right) => new AbsoluteLength(left.Value + right.Value);
		public static AbsoluteLength Subtract(AbsoluteLength left, AbsoluteLength right) => new AbsoluteLength(left.Value - right.Value);
		public static AbsoluteLength Multiply(AbsoluteLength left, AbsoluteLength right) => new AbsoluteLength(left.Value * right.Value);
		public static AbsoluteLength Divide(AbsoluteLength left, AbsoluteLength right) => new AbsoluteLength(left.Value / right.Value);
		public static AbsoluteLength Quotient(AbsoluteLength left, AbsoluteLength right) => new AbsoluteLength(left.Value % right.Value);
		public static AbsoluteLength Add(AbsoluteLength left, double right) => new AbsoluteLength(left.Value + right);
		public static AbsoluteLength Subtract(AbsoluteLength left, double right) => new AbsoluteLength(left.Value - right);
		public static AbsoluteLength Multiply(AbsoluteLength left, double right) => new AbsoluteLength(left.Value * right);
		public static AbsoluteLength Divide(AbsoluteLength left, double right) => new AbsoluteLength(left.Value / right);
		public static AbsoluteLength Quotient(AbsoluteLength left, double right) => new AbsoluteLength(left.Value % right);
		public static AbsoluteLength operator +(AbsoluteLength left, AbsoluteLength right) => Add(left, right);
		public static AbsoluteLength operator -(AbsoluteLength left, AbsoluteLength right) => Subtract(left, right);
		public static AbsoluteLength operator *(AbsoluteLength left, AbsoluteLength right) => Multiply(left, right);
		public static AbsoluteLength operator /(AbsoluteLength left, AbsoluteLength right) => Divide(left, right);
		public static AbsoluteLength operator %(AbsoluteLength left, AbsoluteLength right) => Quotient(left, right);
		public static AbsoluteLength operator +(AbsoluteLength left, double right) => Add(left, right);
		public static AbsoluteLength operator -(AbsoluteLength left, double right) => Subtract(left, right);
		public static AbsoluteLength operator *(AbsoluteLength left, double right) => Multiply(left, right);
		public static AbsoluteLength operator /(AbsoluteLength left, double right) => Divide(left, right);
		public static AbsoluteLength operator %(AbsoluteLength left, double right) => Quotient(left, right);
	}
}