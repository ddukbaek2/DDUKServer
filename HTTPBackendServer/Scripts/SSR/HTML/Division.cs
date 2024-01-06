using System.Collections.Generic;
using Point2Int = DDUKServer.Point2<int>;
using Color3Byte = DDUKServer.Color3<byte>;


namespace DDUKServer.HTML
{
    /// <summary>
    /// 서버사이드렌더링 위젯 - Division.
    /// </summary>
    public class Division : Element
	{
		public override string Tag => "div";

		//public Point2Int Position { set; get; } = Point2Int.Identity; // 
		public Point2Int Size { set; get; } = Point2Int.Identity; // width, height.
		public Color3Byte BackgroundColor { set; get; } = Color3Byte.Identity; // background-color: #FFFFFF
	}
}