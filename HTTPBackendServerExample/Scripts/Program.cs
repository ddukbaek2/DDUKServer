using System;


namespace DDUKServer.HTTPBackendServerExample
{
	public static class Program
	{
		public static void Main(params string[] args)
		{
			var argumentsParser = new ArgumentsParser(args);
			var targetPorts = argumentsParser["-port"];

			if (targetPorts.Count == 0)
				targetPorts.Add("8990");

			var ip = Utility.GetIPAddress();
			var port = int.Parse(targetPorts[0]);

			// CSR.
			//var httpBackendServer = HTTPBackendServer.CreateCSRHTTPBackendServer(ip, port, $"{Utility.GetProjectDirectory()}\\Assets\\CSR");

			// SSR.
			var httpBackendServer = HTTPBackendServer.CreateSSRHTTPBackendServer(ip, port, $"{Utility.GetProjectDirectory()}\\Assets\\SSR");
			//httpBackendServer.SetSessionType(Type.GetType("DDUKServer.HTTPBackendServerExample.ExampleSession, HTTPBackendServerExample"));
			httpBackendServer.SetSessionType(typeof(ExampleSession));
			httpBackendServer.Start();
			httpBackendServer.Shutdown();
		}
	}
}