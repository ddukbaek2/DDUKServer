using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DDUKServer;


namespace DDUKServer
{
	/// <summary>
	/// HTTP File Server.
	/// </summary>
	public class HTTPFileServer : HTTPServer
	{
		private string m_TargetDirectory;

		public HTTPFileServer(string targetDirectory, string ip, int port) : base(ip, port)
		{
			m_TargetDirectory = targetDirectory;
		}

		public override void Start()
		{
			Console.WriteLine($"[SERVER] Target Directory : {m_TargetDirectory}");
			base.Start();
		}

		protected override async Task ProcessRequest(HttpListenerContext context)
		{
			var request = context.Request;
			var requestedEndPoint = request.RemoteEndPoint;
			var httpMethod = request.HttpMethod;
			var url = request.Url;
			Console.WriteLine($"[HFS][{requestedEndPoint.Address}:{requestedEndPoint.Port}][{httpMethod}] {url}");

			var requestedFile = context.Request.Url.AbsolutePath.Substring(1);
			var filepath = Path.Combine(m_TargetDirectory, requestedFile);

			if (!File.Exists(filepath))
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				context.Response.OutputStream.Close();
				Console.WriteLine($"[HFS] File is Not Found : {filepath}");
				return;
			}

			try
			{
				using (var stream = File.OpenRead(filepath))
				{
					// CORS 헤더 설정.
					context.Response.AddHeader("Access-Control-Allow-Origin", "*");

					context.Response.ContentType = "application/octet-stream";
					context.Response.ContentLength64 = stream.Length;

					context.Response.StatusCode = (int)HttpStatusCode.OK;
					stream.CopyTo(context.Response.OutputStream);
					Console.WriteLine($"[HFS] OK : {filepath}");
				}
			}
			catch (Exception exception)
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				Console.WriteLine($"[HFS] Exception : {exception.Message}");
			}

			context.Response.OutputStream.Close();

			await Task.CompletedTask;
		}


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

			var httpFileServer = new HTTPFileServer(targetDirectory, ip, port);
			httpFileServer.Start();
			httpFileServer.Shutdown();
		}
	}
}