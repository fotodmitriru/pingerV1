using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace AppPinger.Protocols.Interfaces.Implements
{
    class TCP : ITCP
    {
        public string Host { get; set; }
        public int Period { get; set; }
        public int Port { get; set; }
        public string DistStorage { get; set; }
        public event DelegatePingCompleted PingCompleted;

        public bool StartPing()
        {
            if (Host.Length == 0)
                throw new ArgumentException("Не указан адрес для пинга!");
            if (Period == 0)
                throw new ArgumentException("Не указан период для пинга!");
            if (Port == 0)
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
                    timeOut = (tcpClient.ConnectAsync(Host, Port).Wait(1000))
                        ? Period * 1000
                        : (Period - 1) * 1000;
                    string replyLog = string.Format("TCP: {0:dd MMMM yyyy HH:mm:ss} {1} {2}", DateTime.Now, Host,
                        (tcpClient.Connected) ? "Success" : "Failed");
                    PingCompleted?.Invoke(replyLog, DistStorage);
                }

                Thread.Sleep(timeOut);
            }
        });
    }
}
