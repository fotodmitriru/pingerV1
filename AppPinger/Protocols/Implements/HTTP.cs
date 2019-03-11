using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppPinger.Protocols.Interfaces;

namespace AppPinger.Protocols.Implements
{
    class HTTP : IBasePingProtocol
    {
        private readonly ConfigProtocol _configProtocol;
        private readonly string _host;
        private readonly int _period;

        public event DelegatePingCompleted PingCompleted;
        private readonly int _validCode;

        public HTTP(ConfigProtocol configProtocol)
        {
            _configProtocol = configProtocol ?? throw new ArgumentNullException(nameof(configProtocol));

            _host = configProtocol.Host;
            _period = configProtocol.Period;
            _validCode = Convert.ToInt32(configProtocol.GetAdditionalAttribute("ValidCode"));
        }
        public bool StartAsyncPing()
        {
            if (_host.Length == 0)
                throw new ArgumentException("Не указан адрес для пинга!");
            if (_period == 0)
                throw new ArgumentException("Не указан период для пинга!");

            StartAsync();
            return true;
        }

        private async Task StartAsync()
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
                        PingCompleted?.Invoke($"Ошибка url {host} для HTTP протокола!", _configProtocol);
                        return;
                    }

                    try
                    {
                        HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
                        httpResponse.EnsureSuccessStatusCode();
                        string replyLog =
                            $"{DateTime.Now:dd MMMM yyyy HH:mm:ss} {host} {((int) httpResponse.StatusCode == _validCode ? "Success" : "Failed")}";

                        PingCompleted?.Invoke(replyLog, _configProtocol);
                    }
                    catch (HttpRequestException e)
                    {
                        string replyLog = $"\nОшибка :{url} - {e.Message}";
                        PingCompleted?.Invoke(replyLog, _configProtocol);
                    }
                }

                Thread.Sleep(_period * 1000);
            }
        }
    }
}
