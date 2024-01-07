using DDUKServer.HTML;
using System;
using System.Collections.Generic;
using System.IO;
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
		private static List<string> s_ResourceFileExtensions = new List<string>()
		{
			".ico",
			".css",
			".js",

			".webp",
			".jpg",
			".jpeg",
			".png",
			".gif",

			".weba",
			".mp3",
			".ogg",

			".webm",
			".mp4",
		};

		private static List<string> s_DocumentFileExtensions = new List<string>()
		{
			".txt",
			".json",
			".php",
			".xml",
			".htm",
			".html",
			".xhtml",
		};

		public SSRSession(HTTPBackendServer server) : base(server)
		{
		}

		protected async override Task<HttpStatusCode> OnProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
		{
			//var filename = request.Url;
			//var requestname = request.RawUrl;

			var requestedFile = request.Url.AbsolutePath.Substring(1).ToLower(); // URL 없이 "/filename.extension" 이런식으로 나옴.
			if (string.IsNullOrEmpty(requestedFile))
				requestedFile = "index.html";

			var acceptHeader = request.Headers["Accept"];
			var bytes = default(byte[]);

			Console.WriteLine(acceptHeader);

			// 헤더 중에서 Server는 바꿔도 내부적으로 "Microsoft-HTTPAPI/2.0" 이 추가되버림.
			// 레지스트리 수정해야한다고 함.
			// https://stackoverflow.com/questions/68983512/remove-microsoft-httpapi-2-0-from-the-response-header-server-self-hosted-http-s
			// https://techcommunity.microsoft.com/t5/iis-support-blog/remove-unwanted-http-response-headers/ba-p/369710
			//response.AddHeader("Server", "DDUKServer/HTTPBackendServer");

			try
			{
				if (acceptHeader.Contains("text/html"))
				{
					var rootElement = new Division
					{
						Children =
						{
							new Division
							{

							},
							new Division
							{
								Children =
								{
									new Paragraph
									{
										Value = "Server Side Rendering",
									},

									//new Image
									//{
									//	Attributes =
									//	{
									//		{ "src", "Assets/zzio.png" },
									//	},
									//},
								},
							}
						},
					};

					HTMLBuilder.BuildLayout(rootElement, out var html, out var css);
					//var document = HTMLBuilder.BuildDocument("HTTPBackendServer", "Server Side Rendering");
					var document = HTMLBuilder.BuildDocument("HTTPBackendServer", css, html);
					bytes = Encoding.UTF8.GetBytes(document);

					response.ContentType = "text/html; charset=utf-8";
					response.ContentEncoding = Encoding.UTF8;

					//response.AddHeader("Date", DateTime.Now.ToString()); // Sun, 07 Jan 2024 15:43:02 GMT
					//response.AddHeader("Server", "Microsoft-HTTPAPI/2.0"); // "Microsoft-HTTPAPI/2.0"

					//response.Cookies

					// GZIP 헤더 설정.
					//response.AddHeader("Content-Encoding", "gzip");

					// 캐시 헤더 설정 - 캐싱안함.
					response.AddHeader("Cache-Control", "no-store"); // 캐시안함. no-store, no-cache
					response.AddHeader("Access-Control-Allow-Origin", "*"); // CORS.
					response.AddHeader("X-Content-Type-Options", "nosniff");


					//foreach (var header in response.Headers)
					//{
					//	Console.WriteLine(header);
					//}
				}
				// 리소스 파일 요청.
				else if (IsFileExtension(requestedFile))
				{
					var extension = Path.GetExtension(requestedFile);

					// 파일 불러오기.
					bytes = await File.ReadAllBytesAsync($"{TargetDirectory}\\{requestedFile}");

					// 헤더설정.
					if (extension == ".ico")
						response.ContentType = "image/vnd.microsoft.icon"; // "image/x-icon";
					else if (extension == ".png")
						response.ContentType = "image/png";
					else if (extension == ".jpg" || extension == ".jpeg")
						response.ContentType = "image/jpeg";
					else if (extension == ".js")
						response.ContentType = "text/javascript";
					else if (extension == ".oga" || extension == ".ogx")
						response.ContentType = "audio/ogg";
					else if (extension == ".ogv")
						response.ContentType = "video/ogg";

					//response.AddHeader("Pragma", "no-cache"); // HTML1.0 캐시 호환성.
					//response.AddHeader("Expires", "0"); // 캐시 만료.
					//response.AddHeader("Cache-Control", "public, max-age=86400, immutable"); // 1일 캐시.
					response.AddHeader("Cache-Control", "no-store");
					response.AddHeader("X-Content-Type-Options", "nosniff"); // 
					response.ContentType = "application/octet-stream"; // 다운로드 대상.
				}
				// 그 외.
				else
				{
					return HttpStatusCode.BadRequest;
				}

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

		public static bool IsFileExtension(string filename)
		{
			var extension = Path.GetExtension(filename);
			if (!s_ResourceFileExtensions.Contains(extension))
				return false;

			return true;
		}
	}
}