using System.Drawing;
using System.Text;


namespace DDUKServer.JS
{
	public class JSBlock
	{
		public JSBlock()
		{
		}
	}

	public class JSFunction
	{
		public JSFunction()
		{
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class Window
	{

	}

	public class Navigator
	{
		//public RelativeLength
	}

	public class JSBuilder
	{
		private StringBuilder m_StringBuilder;

		public JSBuilder()
		{
			m_StringBuilder = new StringBuilder();
		}

		public void AddVariable(string variable)
		{
		}

		public void RemoveVariable(string variable)
		{
		}

		public string BuildJS()
		{
			return string.Empty;
		}
	}
}