using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppPinger.Protocols.Interfaces;

namespace AppPinger.Protocols.Implements
{
    class HTTP : IBasePingProtocol
    {
        private readonly string _host;
        private readonly int _period;

        public event DelegatePingCompleted PingCompleted;
        private readonly int _validCode = 200;
        private readonly string _distStorage;

        public HTTP(ConfigProtocol configProtocol)
        {
            if (configProtocol == null)
                throw new ArgumentNullException(nameof(configProtocol));

            _host = (string)configProtocol.GetAdditionalAttribute("Host");
            _period = Convert.ToInt32(configProtocol.GetAdditionalAttribute("Period"));
            _validCode = Convert.ToInt32(configProtocol.GetAdditionalAttribute("ValidCode"));
            _distStorage = (string)configProtocol.GetAdditionalAttribute("DistStorage");
        }
        public bool StartPing()
        {
            if (_host.Length == 0)
                throw new ArgumentException("Не указан адрес для пинга!");
            if (_period == 0)
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
                    var urlBuilder = new UriBuilder(_host);
                    urlBuilder.Scheme = "http";
                    string host = urlBuilder.ToString();

                    Uri url;
                    if (!Uri.TryCreate(host, UriKind.Absolute, out url))
                    {
                        PingCompleted?.Invoke(string.Format("HTTP: Ошибка url {0} для HTTP протокола!", host), _distStorage);
                        return;
                    }

                    try
                    {
                        HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
                        httpResponse.EnsureSuccessStatusCode();
                        string replyLog = string.Format("HTTP: {0:dd MMMM yyyy HH:mm:ss} {1} {2}", DateTime.Now, host,
                            (int) httpResponse.StatusCode == _validCode ? "Success" : "Failed");

                        PingCompleted?.Invoke(replyLog, _distStorage);
                    }
                    catch (HttpRequestException e)
                    {
                        string replyLog = string.Format("\nHTTP: Ошибка :{0} - {1}", url, e.Message);
                        PingCompleted?.Invoke(replyLog, _distStorage);
                    }
                }

                Thread.Sleep(_period * 1000);
            }
        }
    }
}
