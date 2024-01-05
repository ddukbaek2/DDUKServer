namespace DDUKServer
{
	/// <summary>
	/// 콘솔 애플리케이션.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// 진입점.
		/// </summary>
		public static void Main(string[] args)
		{
			var argumentsParser = new ArgumentsParser(args);
			var targetDirectories = argumentsParser["-dir"];
			var targetPorts = argumentsParser["-port"];

			if (targetDirectories.Count == 0)
			{
				targetDirectories.Add($"{Utility.GetProjectDirectory()}\\Files");
			}

			if (targetPorts.Count == 0)
				targetPorts.Add("8991");

			var targetDirectory = targetDirectories[0];
			var ip = Utility.GetIPAddress();
			var port = int.Parse(targetPorts[0]);

			var httpFileServer = new HTTPFileServer(ip, port, targetDirectory);
			httpFileServer.Start();
			httpFileServer.Shutdown();
		}
	}
}