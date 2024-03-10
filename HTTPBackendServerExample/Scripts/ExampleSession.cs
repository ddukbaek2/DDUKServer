using DDUKServer.HTML;


namespace DDUKServer.HTTPBackendServerExample
{
	public class ExampleSession : SSRSession
	{
		public ExampleSession(HTTPBackendServer server) : base(server)
		{
		}

		protected override Element OnRender()
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
									//{ "background-color", "#FF0000" },
									{ "style", "background-color: #FF0000;" },
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

			return root;
		}
	}
}