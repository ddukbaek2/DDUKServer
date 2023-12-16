using System;
using System.IO;
using System.Net;


namespace DDUKServer
{
	/// <summary>
	/// HTTP File Server.
	/// </summary>
	public class HTTPFileServer
	{
		private HttpListener m_HttpListener;
		private string m_TargetDirectory;
		private string m_IP;
		private int m_Port;

		public HTTPFileServer(string targetDirectory, string ip, int port)
		{
			m_TargetDirectory = targetDirectory;
			m_IP = ip;
			m_Port = port;
			m_HttpListener = new HttpListener();
			m_HttpListener.Prefixes.Add($"http://127.0.0.1:{m_Port}/");
			m_HttpListener.Prefixes.Add($"http://{m_IP}:{port}/");
		}

		public void Start()
		{
			Console.WriteLine($"[HFS] Target Directory : {m_TargetDirectory}");
			Console.WriteLine($"[HFS] IP : {m_IP}");
			Console.WriteLine($"[HFS] Port : {m_Port}");

			m_HttpListener.Start();
			Console.WriteLine($"[HFS] Start.");
			while (true)
			{
				try
				{
					var context = m_HttpListener.GetContext();
					ProcessRequest(context);
				}
				catch (Exception exeption)
				{
					Console.WriteLine($"[HFS] Error: {exeption.Message}");
				}
			}
		}

		private void ProcessRequest(HttpListenerContext context)
		{
			Console.WriteLine($"[HFS][{context.Request.RemoteEndPoint.Address}:{context.Request.RemoteEndPoint.Port}] Request : {context.Request.Url}");

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
					context.Response.ContentType = "application/octet-stream";
					context.Response.ContentLength64 = stream.Length;
					context.Response.AddHeader("Access-Control-Allow-Origin", "*"); // CORS 헤더 설정.

					context.Response.StatusCode = (int)HttpStatusCode.OK;
					stream.CopyTo(context.Response.OutputStream);
					Console.WriteLine($"[HFS] OK : {filepath}");
				}
			}
			catch (Exception ex)
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				Console.WriteLine($"[HFS] Exception : {ex.Message}");
			}

			context.Response.OutputStream.Close();
		}

		public void Stop()
		{
			Console.WriteLine($"[HFS] Stop.");
			m_HttpListener.Stop();
			m_HttpListener.Close();
		}

		public static string GetIPAddress()
		{
			var hostName = Dns.GetHostName();
			var hostEntry = Dns.GetHostEntry(hostName);
			foreach (var address in hostEntry.AddressList)
			{
				if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
					continue;

				return address.ToString();
			}

			return string.Empty;
		}

		public static void Main(string[] args)
		{
			var argumentsParser = new ArgumentsParser(args);
			var targetDirectories = argumentsParser["-dir"];
			var targetPorts = argumentsParser["-port"];

			if (targetDirectories.Count == 0)
				targetDirectories.Add($"{Environment.CurrentDirectory}\\Files");

			if (targetPorts.Count == 0)
				targetPorts.Add("8991");

			var targetDirectory = targetDirectories[0];
			var ip = HTTPFileServer.GetIPAddress();
			var port = int.Parse(targetPorts[0]);

			var httpFileServer = new HTTPFileServer(targetDirectory, ip, port);

			httpFileServer.Start();
		}
	}
}