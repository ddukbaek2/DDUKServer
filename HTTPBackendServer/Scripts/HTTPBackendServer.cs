using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;


namespace DDUKServer
{
	/// <summary>
	/// Rendering Mode.
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
		private string m_TargetDirectory;
		private RenderingMode m_RenderingMode;

		public string TargetDirectory => m_TargetDirectory;
		public RenderingMode RenderingMode => m_RenderingMode;

		public HTTPBackendServer(string ip, int port, string targetDirectory, RenderingMode renderingMode) : base(ip, port)
		{
			m_Sessions = new ConcurrentBag<ISession>();
			m_TargetDirectory = targetDirectory;
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

		protected override async Task OnRequestProcess(HttpListenerContext context)
		{
			var request = context.Request;
			var requestedEndPoint = request.RemoteEndPoint;
			var httpMethod = request.HttpMethod;
			var url = request.Url;
			Console.WriteLine($"[{requestedEndPoint.Address}:{requestedEndPoint.Port}][{httpMethod}] {url}");

			// 클라는 서버에게 파일을 요청할 수 있다.
			// 클라는 서버에게 문서를 요청할 수 있다. (파일과 사실상 동일한 방식이다)
			// 클라는 서버에게 압축 파일을 스트리밍형태로 요청할 수 있다.

			// 요청이 들어올때마다 세션을 생성 혹은 사용이 끝난 세션을 재사용하여 처리.
			// 세션과 요청한 클라이언트는 절대로 동일개체는 아니다. 이전세션을 들고 있어도 의미없다는 이야기.
			var session = PopSessionFromPool();
			await session.ProcessRequest(context);
			PushSessionToPool(session);
		}

		public static HTTPBackendServer CreateCSRHTTPBackendServer(string ip, int port, string targetDirectory)
		{
			return new HTTPBackendServer(ip, port, targetDirectory, RenderingMode.CSR);
		}

		public static HTTPBackendServer CreateSSRHTTPBackendServer(string ip, int port)
		{
			return new HTTPBackendServer(ip, port, string.Empty, RenderingMode.SSR);
		}
	}
}