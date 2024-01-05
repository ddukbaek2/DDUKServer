using System.IO;
using System.Net;
using System.Reflection;


namespace DDUKServer
{
	/// <summary>
	/// Utility.
	/// </summary>
	public static class Utility
	{
		public static string GetProjectDirectory()
		{
			var executeApplicationDirectory = Utility.GetExecuteApplicationDirectory();
			var directoryInformation = new DirectoryInfo(executeApplicationDirectory);
			return directoryInformation.Parent.Parent.Parent.FullName;
		}

		public static string GetExecuteApplicationDirectory()
		{
			var assembly = Assembly.GetExecutingAssembly();
			if (assembly == null)
				return string.Empty;

			var executeApplicationDirectory = Path.GetDirectoryName(assembly.Location);
			return executeApplicationDirectory;
		}

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