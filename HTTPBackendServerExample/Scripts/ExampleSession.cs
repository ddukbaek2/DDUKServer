using DDUKServer.HTML;
using System.Net;
using System.Threading.Tasks;


namespace DDUKServer.HTTPBackendServerExample
{
	public class ExampleSession : SSRSession
	{
		public ExampleSession(HTTPBackendServer server) : base(server)
		{
		}

		protected override Task<HttpStatusCode> OnProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
		{
			var root = new Division
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
								Attributes =
								{
									{ "background-color", "#FF0000" },
								},

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

			return base.OnProcessRequest(request, response);
		}
	}
}