using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;


namespace DDUKServer
{
	/// <summary>
	/// 렌더링 모드.
	/// </summary>
	public enum RenderingMode
	{
		CSR,
		SSR,
	}


	/// <summary>
	/// HTTP Backend Server.
	/// </summary>
	public class HTTPBackendServer : HTTPServer
	{
		private ConcurrentBag<ISession> m_Sessions;
		private RenderingMode m_RenderingMode;

		public RenderingMode RenderingMode => m_RenderingMode;

		public HTTPBackendServer(string ip, int port, RenderingMode renderingMode) : base(ip, port)
		{
			m_Sessions = new ConcurrentBag<ISession>();
			m_RenderingMode = renderingMode;
		}

		private void PushSessionToPool(ISession session)
		{
			// 처리가 끝난 후 다시 풀에 넣는다.
			m_Sessions.Add(session);
		}

		private ISession PopSessionFromPool()
		{
			// 풀에서 가져온다.
			if (!m_Sessions.TryTake(out var session))
			{
				// 없다면 새로 만든다.
				switch (m_RenderingMode)
				{
					case RenderingMode.CSR:
						{
							session = new CSRSession(this);
							break;
						}

					case RenderingMode.SSR:
						{
							session = new SSRSession(this);
							break;
						}
				}
			}

			return session;
		}

		protected override async Task ProcessRequest(HttpListenerContext context)
		{
			var request = context.Request;
			var requestedEndPoint = request.RemoteEndPoint;
			var httpMethod = request.HttpMethod;
			var url = request.Url;
			Console.WriteLine($"[SERVER][{requestedEndPoint.Address}:{requestedEndPoint.Port}][{httpMethod}] {url}");

			// 클라는 서버에게 파일을 요청할 수 있다.
			// 클라는 서버에게 문서를 요청할 수 있다. (파일과 사실상 동일한 방식이다)
			// 클라는 서버에게 압축 파일을 스트리밍형태로 요청할 수 있다.

			// 처리.
			var session = PopSessionFromPool();
			await session.ProcessRequest(context);
			PushSessionToPool(session);
		}

		public static void Main(string[] args)
		{
			var argumentsParser = new ArgumentsParser(args);
			var targetPorts = argumentsParser["-port"];

			if (targetPorts.Count == 0)
				targetPorts.Add("8990");

			var ip = Utility.GetIPAddress();
			var port = int.Parse(targetPorts[0]);

			var httpBackendServer = new HTTPBackendServer(ip, port, RenderingMode.CSR);
			httpBackendServer.Start();
			httpBackendServer.Shutdown();
		}
	}
}