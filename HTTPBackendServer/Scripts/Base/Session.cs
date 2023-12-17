using System.Net;


namespace DDUKServer
{
	/// <summary>
	/// Session.
	/// </summary>
	public abstract class Session : ISession
	{
        private HttpListenerContext m_Context;
        private HTTPBackendServer m_Server;

        public Session(HTTPBackendServer server)
        {
            m_Context = null;
            m_Server = server;
        }

        public void ProcessRequest(HttpListenerContext context)
        {
			m_Context = context;
			var request = m_Context.Request;
			var response = m_Context.Response;
			response.StatusCode = (int)OnProcessRequest(request, response);
			response.Close();
			m_Context = null;
		}

		protected abstract HttpStatusCode OnProcessRequest(HttpListenerRequest request, HttpListenerResponse response);
    }
}