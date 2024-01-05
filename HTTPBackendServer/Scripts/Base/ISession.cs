using System.Net;
using System.Threading.Tasks;


namespace DDUKServer
{
	/// <summary>
	/// 서버로 들어온 요청.
	/// </summary>
	public interface ISession
	{
		Task ProcessRequest(HttpListenerContext context);
	}
}