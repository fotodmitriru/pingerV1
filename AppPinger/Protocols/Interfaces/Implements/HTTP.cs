using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AppPinger.Protocols.Interfaces.Implements
{
    class HTTP : IHTTP
    {
        public string Host { get; set; }
        public int Period { get; set; } = 0;//читать нулевой элемент из массива настроек

        public event DelegatePingCompleted PingCompleted;
        public int ValidCode { get; set; } = 1;//читать первый элемент из массива настроек

        public bool StartPing(ConfigProtocol confProtocol)
        {
            if (confProtocol == null)
                throw new ArgumentException("Не заданы параметры для протокола HTTP!");

            Host = confProtocol.Host ?? throw new NullReferenceException("Параметр Host не задан!");
            if (confProtocol.AdditionalAttributes.Length > 0)
            {
                Period = Convert.ToInt32(confProtocol.AdditionalAttributes[Period]);
                ValidCode = Convert.ToInt32(confProtocol.AdditionalAttributes[ValidCode]);
            }
            else
            {
                throw new NullReferenceException("Не заданы дополнительные параметры (AdditionalAttributes)!");
            }
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
                        PingCompleted?.Invoke(string.Format("HTTP: Ошибка url {0} для HTTP протокола!", Host));
                        return;
                    }

                    try
                    {
                        HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
                        httpResponse.EnsureSuccessStatusCode();
                        string replyLog = string.Format("HTTP: {0:dd MMMM yyyy HH:mm:ss} {1} {2}", DateTime.Now, Host,
                            (int)httpResponse.StatusCode == ValidCode ? "Success" : "Failed");

                        PingCompleted?.Invoke(replyLog);
                    }
                    catch (HttpRequestException e)
                    {
                        string replyLog = string.Format("\nHTTP: Ошибка :{0} - {1}", url, e.Message);
                        PingCompleted?.Invoke(replyLog);
                    }
                }

                Thread.Sleep(Period * 1000);
            }
        }
    }
}
