using System.Collections.Generic;
using Point2Int = DDUKServer.Point2<int>;
using Color3Byte = DDUKServer.Color3<byte>;


namespace DDUKServer.HTML
{
    /// <summary>
    /// 영역 요소.
    /// </summary>
    public class Division : Element
	{
		public override string Name => "div";

		//public Point2Int Position { set; get; } = Point2Int.Identity; // 
		public Point2Int Size { set; get; } = Point2Int.Identity; // width, height.
		public Color3Byte BackgroundColor { set; get; } = Color3Byte.Identity; // background-color: #FFFFFF
	}
}