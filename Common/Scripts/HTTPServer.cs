using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace DDUKServer
{
	/// <summary>
	/// HTTP Server.
	/// </summary>
	public abstract class HTTPServer
	{
		protected HttpListener m_HttpListener; // 윈도우에서는 IOCP 기반.
		protected int m_MaxTasks;
		protected List<Task> m_Tasks;

		protected string m_IP;
		protected int m_Port;

		public HTTPServer(string ip, int port)
		{
			m_MaxTasks = Environment.ProcessorCount;
			m_Tasks = new List<Task>();

			m_IP = ip;
			m_Port = port;

			m_HttpListener = new HttpListener();
			m_HttpListener.Prefixes.Add($"http://127.0.0.1:{m_Port}/");
			m_HttpListener.Prefixes.Add($"http://{m_IP}:{m_Port}/");
		}

		public virtual void Start()
		{
			if (!HttpListener.IsSupported)
			{
				Console.WriteLine($"[SERVER] HttpListener Not Suport");
				return;
			}

			Console.WriteLine($"[SERVER] IP : {m_IP}");
			Console.WriteLine($"[SERVER] Port : {m_Port}");

			m_HttpListener.Start();

			m_Tasks.Clear();
			for (var i = 0; i < m_MaxTasks; ++i)
			{
				var task = Task.Run(() => RequestAsync(this, m_HttpListener));
				m_Tasks.Add(task);
			}

			Console.WriteLine($"[SERVER] Start.");

			WaitForCompletion();
		}

		private void WaitForCompletion()
		{
			do
			{
				for (var i = 0; i < m_Tasks.Count; ++i)
				{
					var task = m_Tasks[i];
					if (task.IsCompleted || task.IsCanceled || task.IsFaulted)
					{
						m_Tasks.RemoveAt(i);
						--i;
					}
				}
			}
			while (m_Tasks.Count > 0);
		}

		public virtual void Shutdown()
		{
			m_HttpListener.Stop();
			m_HttpListener.Close();
			Console.WriteLine($"[SERVER] Shutdown.");
		}

		private static async Task RequestAsync(HTTPServer server, HttpListener httpListener)
		{
			while (httpListener.IsListening)
			{
				try
				{
					var context = httpListener.GetContext();
					await server.ProcessRequest(context);
				}
				catch (HttpListenerException exception)
				{
					Console.WriteLine($"[SERVER] Error: {exception.Message}");
				}
				catch (Exception exeption)
				{
					Console.WriteLine($"[SERVER] Error: {exeption.Message}");
				}
			}
		}

		protected abstract Task ProcessRequest(HttpListenerContext context);
	}
}