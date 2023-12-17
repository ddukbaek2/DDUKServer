using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


namespace DDUKServer
{
	/// <summary>
	/// Session.
	/// </summary>
	public class Session
	{
		private HttpListenerContext m_Context;
		private HTTPBackendServer m_Server;
		private StringBuilder m_StringBuilder;

		public Session(HTTPBackendServer server)
		{
			m_Context = null;
			m_Server = server;

			m_StringBuilder = new StringBuilder();
			m_StringBuilder.Clear();
			m_StringBuilder.AppendLine("<html>");
			m_StringBuilder.AppendLine("	<title>");
			m_StringBuilder.AppendLine("		HTTPBackendServer");
			m_StringBuilder.AppendLine("	</title>");
			m_StringBuilder.AppendLine("	<body>");
			m_StringBuilder.AppendLine("		Backend Rendering");
			m_StringBuilder.AppendLine("	</body>");
			m_StringBuilder.AppendLine("</html>");
		}

		public void ProcessRequest(HttpListenerContext context)
		{
			m_Context = context;
			var request = m_Context.Request;
			var response = context.Response;

			var filename = request.Url;
			var requestname = request.RawUrl;
			
			try
			{
				var html = m_StringBuilder.ToString();
				var bytes = Encoding.UTF8.GetBytes(html);

				response.AddHeader("Access-Control-Allow-Origin", "*"); // CORS 헤더 설정.
				//response.AddHeader("Content-Encoding", "gzip"); // GZIP 헤더 설정.
				response.ContentLength64 = bytes.Length;
				response.OutputStream.Write(bytes, 0, bytes.Length);
				response.StatusCode = (int)HttpStatusCode.OK;

				Console.WriteLine($"[HBS] OK");
			}
			catch (Exception exception)
			{
				response.StatusCode = (int) HttpStatusCode.InternalServerError;
				Console.WriteLine($"[HBS] Exception : {exception.Message}");
			}

			response.OutputStream.Close();
			m_Context = null;
		}
	}
}