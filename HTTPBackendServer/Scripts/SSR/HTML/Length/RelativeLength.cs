using System;


namespace DDUKServer.HTML
{
	/// <summary>
	/// 상대적 길이 (절대적 길이를 포함한 개념).
	/// </summary>
	public struct RelativeLength : ILength
	{
		/// <summary>
		/// 값.
		/// </summary>
		public double Value { set; get; }

		/// <summary>
		/// 단위.
		/// </summary>
		public UnitOfLength UnitOfLength { set; get; }

		public RelativeLength() => Set(default, UnitOfLength.Pixel);
		public RelativeLength(double pixel) => Set(pixel, UnitOfLength.Pixel);
		public RelativeLength(RelativeLength relativeLength) => Set(relativeLength.Value, relativeLength.UnitOfLength);
		//public RelativeLength(AbsoluteLength absoluteLength) => Set(absoluteLength.Value, UnitOfLength.Pixel);
		public RelativeLength(string text) => Set(text);
		public RelativeLength(sbyte value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);
		public RelativeLength(byte value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);
		public RelativeLength(short value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);
		public RelativeLength(ushort value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);
		public RelativeLength(int value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);
		public RelativeLength(uint value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);
		public RelativeLength(long value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);
		public RelativeLength(ulong value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);
		public RelativeLength(float value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);
		public RelativeLength(double value, UnitOfLength unitOfLength) => Set(value, unitOfLength);
		public RelativeLength(decimal value, UnitOfLength unitOfLength) => Set((double)value, unitOfLength);

		/// <summary>
		/// 값 설정.
		/// </summary>
		public void Set(string text)
		{
			if (!LengthHelper.TryParse(text, out var value, out var unitOfLength))
				throw new ArgumentException();

			Value = value;
			UnitOfLength = unitOfLength;
		}

		/// <summary>
		/// 값 설정 (인터페이스 구현).
		/// </summary>
		public void Set(double value, UnitOfLength unitOfLength)
		{
			Value = value;
			UnitOfLength = unitOfLength;
		}

		/// <summary>
		/// 인터페이스 구현체.
		/// </summary>
		UnitOfLength ILength.GetUnitOfLength() => UnitOfLength;

		/// <summary>
		/// 문자열 반환 (인터페이스 구현).
		/// </summary>
		public readonly override string ToString() => LengthHelper.ToString(Value, UnitOfLength);
	}
}