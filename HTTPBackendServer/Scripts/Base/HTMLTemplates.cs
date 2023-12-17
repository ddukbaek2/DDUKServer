namespace DDUKServer
{
	public static class HTMLTemplates
	{
		static HTMLTemplates()
		{
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
	}
}