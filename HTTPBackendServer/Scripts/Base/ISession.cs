using System.Net;


namespace DDUKServer
{
	public interface ISession
	{
		void ProcessRequest(HttpListenerContext context);
	}
}