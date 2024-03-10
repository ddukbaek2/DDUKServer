using System.Collections.Generic;


namespace DDUKServer.HTML
{
	/// <summary>
	/// 스타일.
	/// </summary>
	public class Style
	{
		public string Name { set; get; }
		public Dictionary<string, string> Properties { set; get; }

		public Style()
		{
		}
	}
}