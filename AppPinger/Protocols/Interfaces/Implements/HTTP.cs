using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AppPinger.Protocols.Interfaces.Implements
{
    class HTTP : IHTTP
    {
        public string Host { get; set; }
        public int Period { get; set; }

        public event DelegatePingCompleted PingCompleted;
        public int ValidCode { get; set; } = 200;
        public string DistStorage { get; set; }
        public bool StartPing()
        {
            if (Host.Length == 0)
                throw new ArgumentException("Не указан адрес для пинга!");
            if (Period == 0)
                throw new ArgumentException("Не указан период для пинга!");

            StartAsync();
            return true;
        }

        async Task StartAsync()
        {
            while (true)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var urlBuilder = new UriBuilder(Host);
                    urlBuilder.Scheme = "http";
                    Host = urlBuilder.ToString();

                    Uri url;
                    if (!Uri.TryCreate(Host, UriKind.Absolute, out url))
                    {
                        PingCompleted?.Invoke(string.Format("HTTP: Ошибка url {0} для HTTP протокола!", Host), DistStorage);
                        return;
                    }

                    try
                    {
                        HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
                        httpResponse.EnsureSuccessStatusCode();
                        string replyLog = string.Format("HTTP: {0:dd MMMM yyyy HH:mm:ss} {1} {2}", DateTime.Now, Host,
                            (int) httpResponse.StatusCode == ValidCode ? "Success" : "Failed");

                        PingCompleted?.Invoke(replyLog, DistStorage);
                    }
                    catch (HttpRequestException e)
                    {
                        string replyLog = string.Format("\nHTTP: Ошибка :{0} - {1}", url, e.Message);
                        PingCompleted?.Invoke(replyLog, DistStorage);
                    }
                }

                Thread.Sleep(Period * 1000);
            }
        }
    }
}
