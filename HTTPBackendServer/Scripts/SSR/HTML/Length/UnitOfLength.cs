namespace DDUKServer.HTML
{
	/// <summary>
	/// CSS에서 사용되는 길이의 단위.
	/// 참고 : https://inpa.tistory.com/entry/CSS-%F0%9F%93%9A-%EA%B0%92%EC%9D%98-%EB%8B%A8%EC%9C%84-px-em-rgb-viewport
	/// </summary>
	public enum UnitOfLength
	{
		Invalid = -1, // 유효하지 않은 단위.
		Pixel, // 1/96인치의 절대단위.
		Percentage, // 부모 대비 백분율 크기.
		EM, // 부모, 혹은 자신의 사이즈 대비 몇배수.
		REM, // 루트 기준의 EM.
		VW, // 웹브라우저 너비의 백분율 크기.
		VH, // 웹브라우저 높이의 백분율 크기.
		VMIN, // 웹브라우저 너비와 높이 중 작은 것의 백분율 크기.
		VMAX, // 웹브라우저 너비와 높이 중 큰 것의 백분율 크기.
	}

	/// <summary>
	/// CSS에서 사용되는 절대적 길이의 단위.
	/// </summary>
	public enum UnitOfAbsoluteLength
	{
		Invalid = -1, // 유효하지 않은 단위.
		Pixel, // 1픽셀 = 1/96인치.
		Point, // 1포인트 = 1/72인치.
		Pica, // 1파이카 = 12포인트.
		IN, // 인치.
		CM, // 센티미터.
		MM, // 밀리미터.
	}

	/// <summary>
	/// CSS에서 사용되는 절대적 길이의 단위.
	/// </summary>
	public enum UnitOfRelativeLength
	{
		Invalid = -1, // 유효하지 않은 단위.
		Percentage,
		EM,
		REM,
		VW,VH,
		VMIN,
		VMAX,
	}
}