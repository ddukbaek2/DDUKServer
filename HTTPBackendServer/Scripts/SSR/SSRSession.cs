using DDUKServer.HTML;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace DDUKServer
{
    /// <summary>
    /// 서버사이드렌더링 세션.
    /// </summary>
    public class SSRSession : Session
	{
		public SSRSession(HTTPBackendServer server) : base(server)
		{
		}

		protected async override Task<HttpStatusCode> OnProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
		{
			var filename = request.Url;
			var requestname = request.RawUrl;

			try
			{
				var rootElement = new Division
				{
					Children =
					{
						new Division
						{
							Children =
							{
								new Paragraph
								{
									Value = "Server Side Rendering",
								},
							},
						}
					},
				};

				HTMLBuilder.BuildLayout(rootElement, out var html, out var css);
				//var document = HTMLBuilder.BuildDocument("HTTPBackendServer", "Server Side Rendering");
				var document = HTMLBuilder.BuildDocument("HTTPBackendServer", css, html);
				var bytes = Encoding.UTF8.GetBytes(document);

				response.AddHeader("Access-Control-Allow-Origin", "*"); // CORS 헤더 설정.
				//response.AddHeader("Content-Encoding", "gzip"); // GZIP 헤더 설정.

				response.ContentLength64 = bytes.Length;
				await response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
				Console.WriteLine($"[HBS] OK");

				return HttpStatusCode.OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine($"[HBS] Exception : {exception.Message}");
				return HttpStatusCode.InternalServerError;
			}
		}
	}
}