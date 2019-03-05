using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using AppPinger.Protocols.Interfaces;

namespace AppPinger.Protocols.Implements
{
    class TCP : IBasePingProtocol
    {
        private readonly string _host;
        private readonly int _period;
        private readonly int _port;
        private readonly string _distStorage;
        public event DelegatePingCompleted PingCompleted;

        public TCP(ConfigProtocol configProtocol)
        {
            if (configProtocol == null)
                throw new ArgumentNullException(nameof(configProtocol));

            _host = (string)configProtocol.GetAdditionalAttribute("Host");
            _period = Convert.ToInt32(configProtocol.GetAdditionalAttribute("Period"));
            _port = Convert.ToInt32(configProtocol.GetAdditionalAttribute("Port"));
            _distStorage = (string)configProtocol.GetAdditionalAttribute("DistStorage");
        }

        public bool StartPing()
        {
            if (_host.Length == 0)
                throw new ArgumentException("Не указан адрес для пинга!");
            if (_period == 0)
                throw new ArgumentException("Не указан период для пинга!");
            if (_port == 0)
                throw new ArgumentException("Не указан порт для пинга!");

            StartAsync();
            return true;
        }

        async Task StartAsync() => await Task.Run(() =>
        {
            while (true)
            {
                int timeOut;
                using (TcpClient tcpClient = new TcpClient())
                {
                    timeOut = (tcpClient.ConnectAsync(_host, _port).Wait(1000))
                        ? _period * 1000
                        : (_period - 1) * 1000;
                    string replyLog = string.Format("TCP: {0:dd MMMM yyyy HH:mm:ss} {1} {2}", DateTime.Now, _host,
                        (tcpClient.Connected) ? "Success" : "Failed");
                    PingCompleted?.Invoke(replyLog, _distStorage);
                }

                Thread.Sleep(timeOut);
            }
        });
    }
}
