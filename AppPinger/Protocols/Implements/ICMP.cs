using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppPinger.Protocols.Interfaces;

namespace AppPinger.Protocols.Implements
{
    public class ICMP : IBasePingProtocol
    {
        private readonly ConfigProtocol _configProtocol;
        private readonly string _host;
        private readonly int _period;

        public ICMP(ConfigProtocol configProtocol)
        {
            _configProtocol = configProtocol ?? throw new ArgumentNullException(nameof(configProtocol));

            _host = configProtocol.Host;
            _period = configProtocol.Period;
        }

        public bool StartAsyncPing()
        {
            if (string.IsNullOrEmpty(_host))
                throw new ArgumentException("Не указан адрес для пинга!");
            if (_period == 0)
                throw new ArgumentException("Не указан период для пинга!");

            StartAsync();
            return true;
        }

        private async Task StartAsync() => await Task.Run(StartPing);

        private Task StartPing()
        {
            while (true)
            {
                AutoResetEvent waiter = new AutoResetEvent(false);
                using (Ping pingSender = new Ping())
                {
                    pingSender.PingCompleted += PingRoundCompleted;
                    string dataBuff = new string('a', 32);
                    byte[] buffer = Encoding.ASCII.GetBytes(dataBuff);

                    PingOptions pingOptions = new PingOptions(64, true);
                    int timeout = 1000;

                    pingSender.SendAsync(_host, timeout, buffer, pingOptions, waiter);
                }

                Thread.Sleep(_period * 1000);
            }
        }

        public event DelegatePingCompleted PingCompleted;

        private void PingRoundCompleted(object sender, PingCompletedEventArgs ev)
        {
            if (ev.Cancelled)
            {
                PingCompleted?.Invoke("Пинг отменён.", _configProtocol);
                ((AutoResetEvent) ev.UserState).Set();
            }

            if (ev.Error != null)
            {
                PingCompleted?.Invoke($"Ошибка пинга: {ev.Error}", _configProtocol);
                ((AutoResetEvent) ev.UserState).Set();
            }

            PingReply pingReply = ev.Reply;
            var replyLog = $"{DateTime.Now:dd MMMM yyyy HH:mm:ss} {_host} {pingReply.Status}";
            ((AutoResetEvent) ev.UserState).Set();
            PingCompleted?.Invoke(replyLog, _configProtocol);
        }
    }
}
