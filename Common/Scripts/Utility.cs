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
		/// <summary>
		/// 현재 프로젝트의 루트 디렉토리 경로를 반환.
		/// 프로그램 실행 지점으로부터 세번 위로 올라가면 CSPROJ가 존재하는 위치라고 가정함.
		/// </summary>
		public static string GetProjectDirectory()
		{
			var executeApplicationDirectory = Utility.GetExecuteApplicationDirectory();
			var directoryInformation = new DirectoryInfo(executeApplicationDirectory);
			return directoryInformation.Parent.Parent.Parent.FullName;
		}

		/// <summary>
		/// 현재 프로그램이 실행되는 디렉토리 경로를 반환.
		/// </summary>
		public static string GetExecuteApplicationDirectory()
		{
			var assembly = Assembly.GetExecutingAssembly();
			if (assembly == null)
				return string.Empty;

			var executeApplicationDirectory = Path.GetDirectoryName(assembly.Location);
			return executeApplicationDirectory;
		}

		/// <summary>
		/// 현재 프로그램이 실행되는 장비의 아이피를 반환.
		/// </summary>
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