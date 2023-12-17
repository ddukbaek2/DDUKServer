using System;
using System.Net;
using System.Text;


namespace DDUKServer
{
	public class SSRSession : Session
	{
		public SSRSession(HTTPBackendServer server) : base(server)
		{
		}

		protected override HttpStatusCode OnProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
		{
			var filename = request.Url;
			var requestname = request.RawUrl;

			try
			{
				var html = HTMLTemplates.Default("HTTPBackendServer", "Server Side Rendering");
				var bytes = Encoding.UTF8.GetBytes(html);

				response.AddHeader("Access-Control-Allow-Origin", "*"); // CORS 헤더 설정.
																		//response.AddHeader("Content-Encoding", "gzip"); // GZIP 헤더 설정.
				response.ContentLength64 = bytes.Length;
				response.OutputStream.Write(bytes, 0, bytes.Length);
				Console.WriteLine($"[HBS] OK");

				return HttpStatusCode.OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine($"[HBS] Exception : {exception.Message}");

				return HttpStatusCode.InternalServerError;
			}

			//return HttpStatusCode.OK;
		}
	}
}