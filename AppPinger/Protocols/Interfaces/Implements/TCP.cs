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
        public int Port { get; set; } = 1;
        public event DelegatePingCompleted PingCompleted;

        public bool StartPing(ConfigProtocol confProtocol)
        {
            if (confProtocol == null)
                throw new ArgumentException("Не заданы параметры для протокола TCP!");

            Host = confProtocol.Host ?? throw new NullReferenceException("Параметр Host не задан!");
            if (confProtocol.AdditionalAttributes != null && confProtocol.AdditionalAttributes.Length > 0)
            {
                Period = Convert.ToInt32(confProtocol.AdditionalAttributes[Period]);
                Port = Convert.ToInt32(confProtocol.AdditionalAttributes[Port]);
            }
            else
            {
                throw new NullReferenceException("Не заданы дополнительные параметры (AdditionalAttributes)!");
            }
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
                                ? Period * 1000 : (Period - 1) * 1000;
                    string replyLog = string.Format("TCP: {0:dd MMMM yyyy HH:mm:ss} {1} {2}", DateTime.Now, Host,
                        (tcpClient.Connected) ? "Success" : "Failed");
                    PingCompleted?.Invoke(replyLog);
                }

                Thread.Sleep(timeOut);
            }
        });
    }
}
