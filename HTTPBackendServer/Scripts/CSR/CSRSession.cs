using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace DDUKServer
{
	/// <summary>
	/// 클라이언트사이드렌더링 세션.
	/// 걍 파일을 요청하면 파일을 준다.
	/// </summary>
	public class CSRSession : Session
	{
		public CSRSession(HTTPBackendServer server) : base(server)
		{
		}

		protected async override Task<HttpStatusCode> OnProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
		{
			//var request = context.Request;
			var requestedEndPoint = request.RemoteEndPoint;
			var httpMethod = request.HttpMethod;
			var url = request.Url;

			var requestname = request.RawUrl;
			var requestedFile = request.Url.AbsolutePath.Substring(1);
			if (string.IsNullOrEmpty(requestedFile))
				requestedFile = "index.html";

			var filepath = $"{TargetDirectory}\\{requestedFile}";

			try
			{				
				var html = await File.ReadAllTextAsync(filepath);
				var bytes = Encoding.UTF8.GetBytes(html);

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