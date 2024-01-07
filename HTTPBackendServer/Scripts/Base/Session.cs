using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;


namespace DDUKServer
{
	/// <summary>
	/// Session.
	/// </summary>
	public abstract class Session : ManagedObject, ISession
	{
        private HttpListenerContext m_Context;
        private HTTPBackendServer m_Server;

		public string TargetDirectory => m_Server.TargetDirectory;

        public Session(HTTPBackendServer server) : base()
        {
            m_Context = null;
            m_Server = server;
        }

        public async Task ProcessRequest(HttpListenerContext context)
        {
			m_Context = context;
			var request = m_Context.Request;
			var response = m_Context.Response;
			response.StatusCode = (int) await OnProcessRequest(request, response);
			response.Close();
			m_Context = null;
		}

		protected abstract Task<HttpStatusCode> OnProcessRequest(HttpListenerRequest request, HttpListenerResponse response);
    }
}