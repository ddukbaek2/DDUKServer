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
			var targetPorts = argumentsParser["-port"];

			if (targetPorts.Count == 0)
				targetPorts.Add("8990");

			var ip = Utility.GetIPAddress();
			var port = int.Parse(targetPorts[0]);

			// CSR.
			var httpBackendServer = HTTPBackendServer.CreateCSRHTTPBackendServer(ip, port, $"{Utility.GetProjectDirectory()}\\Assets\\CSR");

			// SSR.
			//var httpBackendServer = HTTPBackendServer.CreateSSRHTTPBackendServer(ip, port);
			httpBackendServer.Start();
			httpBackendServer.Shutdown();
		}
	}
}