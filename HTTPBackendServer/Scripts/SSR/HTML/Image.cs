using System.Collections.Generic;
using Point2Int = DDUKServer.Point2<int>;
using Color3Byte = DDUKServer.Color3<byte>;


namespace DDUKServer.HTML
{
	/// <summary>
	/// 이미지 요소.
	/// </summary>
	public class Image : Element
	{
		public override string Name => "img";
		public string Source { set; get; }
		public string Width { set; get; }
		public string Height { set; get; }
		public string BackgroundColor { set; get; } // background-color: #FFFFFF
	}
}