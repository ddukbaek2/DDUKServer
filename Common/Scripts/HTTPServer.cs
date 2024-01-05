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

		protected HTTPServer(string ip, int port)
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
			Console.WriteLine($"[SERVER] Max Task : {m_MaxTasks}");

			m_HttpListener.Start();

			// 세션의 수는 코어갯수와 동일하다.
			m_Tasks.Clear();
			for (var i = 0; i < m_MaxTasks; ++i)
			{
				var task = Task.Run(() => RequestProcessAsync(this, m_HttpListener));
				m_Tasks.Add(task);
			}

			Console.WriteLine($"[SERVER] Start.");

			WaitForCompletion();
		}

		/// <summary>
		/// 요청이 남아있다면 무한 대기.
		/// </summary>
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

		/// <summary>
		/// 끄기.
		/// </summary>
		public virtual void Shutdown()
		{
			m_HttpListener.Stop();
			m_HttpListener.Close();
			Console.WriteLine($"[SERVER] Shutdown.");
		}

		/// <summary>
		/// 요청에 대한 처리.
		/// </summary>
		private static async Task RequestProcessAsync(HTTPServer server, HttpListener httpListener)
		{
			while (httpListener.IsListening)
			{
				try
				{
					// GetContext() 와 OnRequestProcess()를 구분처리.
					// 즉, OnRequestProcess() 실행 중에도 GetContext()는 코어 갯수만큼 호출된다.
					// 동시 처리량을 증가시키기 위한 아이디어.
					// 내부적으로 만약 response

					// 대기중에 요청이 들어오면 받음.
					var context = httpListener.GetContext();
					//httpListener.BeginGetContext(); // 어쩌면 이게 더 나은 대안일 수 있음. 비동기 요청 대기용 함수.

					// 받은 요청에 대해 비동기 처리.
					await server.OnRequestProcess(context);
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

		/// <summary>
		/// 실제 요청 처리.
		/// </summary>
		protected abstract Task OnRequestProcess(HttpListenerContext context);
	}
}