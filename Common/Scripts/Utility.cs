using System.Net;


namespace DDUKServer
{
	/// <summary>
	/// Utility.
	/// </summary>
	public static class Utility
	{
		public static string GetIPAddress()
		{
			var hostName = Dns.GetHostName();
			var hostEntry = Dns.GetHostEntry(hostName);
			foreach (var address in hostEntry.AddressList)
			{
				if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
					continue;

				return address.ToString();
			}

			return string.Empty;
		}
	}
}