using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace MiniWebServer.Core
{
    public class MyHttpServer
    {
        private readonly string[] IndexFileNames = { "index.html", "default.html" };

        private static IDictionary<string, string> MimeTypeMapping = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
            {".css", "text/css"},
            {".html", "text/html"},
            {".jpg", "image/jpeg"},
            {".js", "application/x-javascript"}
        };

        private Thread _listeningThread;

        private HttpListener _listener;

        public int Port { get; private set; }

        public string RootDirectory { get; private set; }

        public bool IsRunning { get; private set; }

        public MyHttpServer(string path, int port)
        {
            this.RootDirectory = path;
            this.Port = port;
        }

        public void Stop()
        {
            if (IsRunning)
            {
                _listener.Stop();
                _listener.Close();
                _listeningThread.Abort();
                IsRunning = false;
            }
        }

        public void Start()
        {
            if (!IsRunning)
            {
                _listeningThread = new Thread(this.Listen);
                _listeningThread.Start();
                IsRunning = true;
            }
        }

        public void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + Port + "/");

            _listener.Start();

            while (_listener.IsListening)
            {
                var context = _listener.GetContext();
                ProcessRequest(context);
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var filename = GetFullPathRequestedFile(context);

            if (File.Exists(filename))
            {
                try
                {
                    using (Stream input = new FileStream(filename, FileMode.Open))
                    {
                        context.Response.ContentType = GetContentType(filename);
                        context.Response.ContentLength64 = input.Length;
                        context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                        context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(filename).ToString("r"));

                        byte[] buffer = new byte[1024 * 16];
                        int bytes;
                        while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            context.Response.OutputStream.Write(buffer, 0, bytes);
                        }

                        input.Close();
                    }

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Flush();
                }
                catch
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
        }

        private static string GetContentType(string filename)
        {
            var extension = Path.GetExtension(filename);
            return MimeTypeMapping.ContainsKey(extension) ? MimeTypeMapping[extension] : "application/octet-stream";
        }

        private string GetFullPathRequestedFile(HttpListenerContext context)
        {
            var filename = context.Request.Url.AbsolutePath;

            if (string.IsNullOrEmpty(filename) || filename == "/")
            {
                foreach (string indexFile in IndexFileNames)
                {
                    var fullPath = Path.Combine(RootDirectory, indexFile);
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
            }

            return Path.Combine(RootDirectory, filename.TrimStart('/'));
        }
    }
}