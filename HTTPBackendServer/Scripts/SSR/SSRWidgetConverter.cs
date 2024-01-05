using System.Text;


namespace DDUKServer
{
	/// <summary>
	/// 위젯 변환기.
	/// </summary>
	public static class SSRWidgetConverter
	{
		private static StringBuilder s_StringBuilder;

		static SSRWidgetConverter()
		{
			s_StringBuilder = new StringBuilder();
		}

		public static string Default(string title, string body)
		{
			return $@"
						<!DOCTYPE html>
						<html>
							<title>
								{title}
							</title>
							<body>
								{body}
							</body>
						</html>
					";
		}

		public static string Build(SSRWidget widget)
		{
			s_StringBuilder.Clear();

			//widget...

			return s_StringBuilder.ToString();
		}
	}
}
