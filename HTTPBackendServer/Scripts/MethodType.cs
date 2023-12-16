namespace DDUKServer
{
	/// <summary>
	/// Method Type.
	/// </summary>
	public enum MethodType
	{
		Get, // 조회.
		Post, // 생성.
		Put, // 생성 혹은 수정 - 여러번 수행해도 동일한 결과.
		//Patch, // 일부만 수정.
		Delete, // 삭제.
		//Head, // 조회이지만 응답해더만을 반환.
		//Options, // 메서드의 종류를 반환.
		//Connect, // 연결.
		//Trace, // 요청을 추적.
	}
}
