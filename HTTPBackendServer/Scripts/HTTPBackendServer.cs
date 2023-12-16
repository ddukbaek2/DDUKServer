using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


namespace DDUKServer
{
	/// <summary>
	/// HTTP Backend Server.
	/// </summary>
	public class HTTPBackendServer
	{
		private HttpListener m_HttpListener;
		private CancellationTokenSource m_CancellationTokenSource;
		private ConcurrentBag<Session> m_Sessions;

		private string m_IP;
		private int m_Port;

		public HTTPBackendServer(string ip, int port)
		{
			m_IP = ip;
			m_Port = port;
			m_HttpListener = new HttpListener();
			m_HttpListener.Prefixes.Add($"http://127.0.0.1:{m_Port}/");
			m_HttpListener.Prefixes.Add($"http://{m_IP}:{port}/");

			m_Sessions = new ConcurrentBag<Session>();
		}

		public void Start()
		{
			Console.WriteLine($"[HBS] IP : {m_IP}");
			Console.WriteLine($"[HBS] Port : {m_Port}");

			m_HttpListener.Start();
			Console.WriteLine($"[HBS] Start.");
			while (true)
			{
				try
				{
					var context = m_HttpListener.GetContext();
					ProcessRequest(context);
				}
				catch (Exception exeption)
				{
					Console.WriteLine($"[HBS] Error: {exeption.Message}");
				}
			}
		}

		private void ProcessRequest(HttpListenerContext context)
		{
			Console.WriteLine($"[HBS][{context.Request.RemoteEndPoint.Address}:{context.Request.RemoteEndPoint.Port}] Request : {context.Request.Url}");

			// 풀에서 가져온다.
			if (!m_Sessions.TryTake(out var session))
			{
				// 없다면 새로 만든다.
				session = new Session(this);
			}

			// 클라는 서버에게 파일을 요청할 수 있다.
			// 클라는 서버에게 문서를 요청할 수 있다. (파일과 사실상 동일한 방식이다)
			// 클라는 서버에게 압축 파일을 스트리밍형태로 요청할 수 있다.

			Task.Run(() =>
			{
				session.ProcessRequest(context);
				m_Sessions.Add(session); // 처리가 끝난 후 다시 풀에 넣는다.
			});
		}

		public void Stop()
		{
			Console.WriteLine($"[HBS] Stop.");
			m_HttpListener.Stop();
			m_HttpListener.Close();
		}

		public static void Main(string[] args)
		{
			var argumentsParser = new ArgumentsParser(args);
			var targetPorts = argumentsParser["-port"];

			if (targetPorts.Count == 0)
				targetPorts.Add("8990");

			var ip = Utility.GetIPAddress();
			var port = int.Parse(targetPorts[0]);

			var httpBackendServer = new HTTPBackendServer(ip, port);

			httpBackendServer.Start();
		}
	}
}