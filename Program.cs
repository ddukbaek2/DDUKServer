using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

class HttpFileServer
{
    private HttpListener m_HttpListener;
    private string m_BaseFolder;
    private string m_IP;
    private int m_Port;

    public HttpFileServer(string baseFolder, int port)
    {
        m_BaseFolder = baseFolder;
        m_IP = HttpFileServer.GetIPAddress();
        m_Port = port;
        m_HttpListener = new HttpListener();
        m_HttpListener.Prefixes.Add($"http://127.0.0.1:{m_Port}/");
        m_HttpListener.Prefixes.Add($"http://{m_IP}:{port}/");
        //m_HttpListener.Prefixes.Add($"http://{m_IP}:{m_Port}/");
        //m_HttpListener.Prefixes.Add($"http://10.190.140.95:{port}/");
    }

    public void Start()
    {
        Console.WriteLine($"Server BaseFolder : {m_BaseFolder}");
        Console.WriteLine($"Server BaseFolder : {m_IP}");
        Console.WriteLine($"Server Port : {m_Port}");
        m_HttpListener.Start();
        Console.WriteLine($"Server Start.");
        while (true)
        {
            try
            {
                var context = m_HttpListener.GetContext();
                ProcessRequest(context);
            }
            catch (Exception exeption)
            {
                Console.WriteLine($"Error: {exeption.Message}");
            }
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        Console.WriteLine($"[{context.Request.RemoteEndPoint.Address}:{context.Request.RemoteEndPoint.Port}] Request : {context.Request.Url}");

        var requestedFile = context.Request.Url.AbsolutePath.Substring(1);
        var filepath = Path.Combine(m_BaseFolder, requestedFile);

        if (!File.Exists(filepath))
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.OutputStream.Close();
            Console.WriteLine($"Not Found : {filepath}");
            return;
        }

        try
        {
            using (var stream = File.OpenRead(filepath))
            {
                context.Response.ContentType = "application/octet-stream";
                context.Response.ContentLength64 = stream.Length;
                context.Response.AddHeader("Access-Control-Allow-Origin", "*"); // CORS 헤더 설정.
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                stream.CopyTo(context.Response.OutputStream);
                Console.WriteLine($"OK : {filepath}");
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Console.WriteLine($"Exception : {ex.Message}");
        }

        context.Response.OutputStream.Close();
    }

    public void Stop()
    {
        Console.WriteLine($"Server Stop.");
        m_HttpListener.Stop();
        m_HttpListener.Close();
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

    static void Main(string[] args)
    {
        // DESKTOP-I2UJ2LD
        // WORKGROUP
        // Acts29
        // 패스트파이브는 다음의 방법을 통해 도메인을 예약해야 서버가 열림.
        // netsh http add urlacl url = http://127.0.0.1:8991/ user=Acts29 
        // netsh http add urlacl url = http://10.190.140.95:8991/ user=Acts29
        // 외부에서 접속하려면 추가로 방화벽도 꺼야함.

        // http://localhost:8991/Boutique_Teaser_04.mp4
        // http://10.190.140.95:8991/Boutique_Teaser_04.mp4

        var baseFolder = args.Length > 0 ? args[0] : "D:\\Files";
        var port = args.Length > 1 ? int.Parse(args[1]) : 8991;

        var httpFileServer = new HttpFileServer(baseFolder, port);
        httpFileServer.Start();
    }
}
