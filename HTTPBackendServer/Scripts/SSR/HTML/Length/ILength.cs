namespace DDUKServer.HTML
{
	/// <summary>
	/// 길이 인터페이스.
	/// </summary>
	public interface ILength
	{
		/// <summary>
		/// 값.
		/// </summary>
		double Value { set; get; }

		/// <summary>
		/// 설정.
		/// </summary>
		void Set(double value, UnitOfLength unitOfLength);

		/// <summary>
		/// 단위 반환.
		/// </summary>
		UnitOfLength GetUnitOfLength();

		/// <summary>
		/// 문자열 반환.
		/// </summary>
		string ToString();
	}
}