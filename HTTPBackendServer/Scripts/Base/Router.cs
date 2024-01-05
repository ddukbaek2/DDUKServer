using System.Collections.Generic;

namespace DDUKServer
{
	/// <summary>
	/// 라우터.
	/// </summary>
	public class Router
	{
		/// <summary>
		/// 이동 가능한 페이지.
		/// </summary>
		private Dictionary<string, string> m_Data;

		public Router()
		{
			m_Data = new Dictionary<string, string>();
		}

		public void Add(string path, string pageName)
		{
			m_Data.Add(path, pageName);
		}
	}
}
