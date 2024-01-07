namespace DDUKServer.HTML
{
	/// <summary>
	/// 링크 요소.
	/// </summary>
	public class Anchor : Element
	{
		public override string Name => "a";
		public string URL { set; get; }
		public string Target { set; get; } // _self, _blank, _parent, _top, TargetName.
	}
}