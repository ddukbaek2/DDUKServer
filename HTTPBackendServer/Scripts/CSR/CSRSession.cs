using DDUKServer;
using System.Net;


namespace DDUKServer
{
	public class CSRSession : Session
	{
		public CSRSession(HTTPBackendServer server) : base(server)
		{
		}

		protected override HttpStatusCode OnProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
		{
			return HttpStatusCode.OK;
		}
	}
}