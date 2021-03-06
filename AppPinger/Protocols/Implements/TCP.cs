﻿using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using AppPinger.Protocols.Interfaces;

namespace AppPinger.Protocols.Implements
{
    public class TCP : IBasePingProtocol
    {
        private readonly ConfigProtocol _configProtocol;
        private readonly string _host;
        private readonly int _period;
        private readonly int _port;
        public event DelegatePingCompleted PingCompleted;

        public TCP(ConfigProtocol configProtocol)
        {
            _configProtocol = configProtocol ?? throw new ArgumentNullException(nameof(configProtocol));

            _host = configProtocol.Host;
            _period = configProtocol.Period;
            _port = Convert.ToInt32(configProtocol.GetAdditionalAttribute("Port"));
        }

        public bool StartAsyncPing()
        {
            if (string.IsNullOrEmpty(_host))
                throw new ArgumentException("Не указан адрес для пинга!");
            if (_period == 0)
                throw new ArgumentException("Не указан период для пинга!");
            if (_port == 0)
                throw new ArgumentException("Не указан порт для пинга!");

            Task.Run(StartPing);
            return true;
        }

        private Task StartPing()
        {
            while (true)
            {
                int timeOut;
                using (TcpClient tcpClient = new TcpClient())
                {
                    timeOut = (tcpClient.ConnectAsync(_host, _port).Wait(1000))
                        ? _period * 1000
                        : (_period - 1) * 1000;
                    string replyLog = $"{DateTime.Now:dd MMMM yyyy HH:mm:ss} {_host} {((tcpClient.Connected) ? "Success" : "Failed")}";
                    PingCompleted?.Invoke(replyLog, _configProtocol);
                }

                Thread.Sleep(timeOut);
            }
        }
    }
}
