namespace DDUKServer.HTML
{
	public class Anchor : Element
	{
		public override string Tag => "a";
		public string Href { set; get; }
		public string Target { set; get; } // _self, _blank, _parent, _top, TargetName.
	}
}