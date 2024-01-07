using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace DDUKServer
{
	/// <summary>
	/// 클라이언트사이드렌더링 세션.
	/// 걍 파일을 요청하면 파일을 읽어서 정보를 준다.
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
			var requestedFile = request.Url.AbsolutePath.Substring(1); // URL 없이 "/filename.extension" 이런식으로 나옴.
			if (string.IsNullOrEmpty(requestedFile))
				requestedFile = "index.html";

			var filepath = $"{TargetDirectory}\\{requestedFile}";

			try
			{
				var bytes = default(byte[]);
				if (filepath.EndsWith(".html"))
				{
					var html = await File.ReadAllTextAsync(filepath);
					bytes = Encoding.UTF8.GetBytes(html);
					response.ContentType = "text/html";
					response.AddHeader("Access-Control-Allow-Origin", "*"); // CORS 헤더 설정.
				}
				else
				{
					bytes = await File.ReadAllBytesAsync(filepath);
					response.ContentType = "application/octet-stream"; // 다운로드 대상.
					response.AddHeader("Content-Disposition", $"attachment; filename={requestedFile}");
				}

				//response.AddHeader("Content-Encoding", "gzip"); // GZIP 헤더 설정.

				response.ContentLength64 = bytes.Length;
				await response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
				response.OutputStream.Close();

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