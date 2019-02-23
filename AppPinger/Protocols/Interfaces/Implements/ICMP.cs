using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppPinger.Protocols.Interfaces.Implements
{
    class ICMP : IICMP
    {
        public string Host { get; set; }
        public int Period { get; set; }
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

        async Task StartAsync() => await Task.Run(() =>
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

                    pingSender.SendAsync(Host, timeout, buffer, pingOptions, waiter);
                }

                Thread.Sleep(Period * 1000);
            }
        });

        public event DelegatePingCompleted PingCompleted;

        private void PingRoundCompleted(object sender, PingCompletedEventArgs ev)
        {
            string replyLog;
            if (ev.Cancelled)
            {
                PingCompleted?.Invoke("ICMP: Пинг отменён.", DistStorage);
                ((AutoResetEvent) ev.UserState).Set();
            }

            if (ev.Error != null)
            {
                PingCompleted?.Invoke(string.Format("ICMP: Ошибка пинга: {0}", ev.Error), DistStorage);
                ((AutoResetEvent) ev.UserState).Set();
            }

            PingReply pingReply = ev.Reply;
            replyLog = string.Format("ICMP: {0} {1} {2}", DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss"), Host,
                pingReply.Status);
            ((AutoResetEvent)ev.UserState).Set();
            PingCompleted?.Invoke(replyLog, DistStorage);
        }
    }
}
