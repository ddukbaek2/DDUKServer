using System.Collections.Generic;

namespace DDUKServer.HTML
{
	/// <summary>
	/// Cascading Style Sheets.
	/// </summary>
	public class CSS
	{
		public string Name = string.Empty;
		public Dictionary<string, string> Properties { set; get; } = new Dictionary<string, string>();
	}
}