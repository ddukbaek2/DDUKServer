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
			return base.OnProcessRequest(request, response);
		}
	}
}