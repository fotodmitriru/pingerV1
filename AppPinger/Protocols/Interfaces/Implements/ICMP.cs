using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace AppPinger.Protocols.Interfaces.Implements
{
    class ICMP : IICMP
    {
        public string Host { get; set; }
        public int Period { get; set; } = 1;
        public string ReplyLog { get; set; }
        //public IList<ConfigProtocol> ListConfProtocols { get; set; }

        public bool StartPing(ConfigProtocol confProtocol)
        {
            ///////из конструктора///
            if (confProtocol == null)
                throw new ArgumentException("Не заданы параметры для протокола ICMP!");
            Host = confProtocol.Host ?? throw new NullReferenceException("Параметр Host не задан!");
            Period = confProtocol.Period;
            ////////////////////////

            if (Host.Length == 0)
                throw new ArgumentException("Не указан адрес для пинга!");

            AutoResetEvent waiter = new AutoResetEvent(false);
            Ping pingSender = new Ping();
            pingSender.PingCompleted += PingRoundCompleted;
            string dataBuff = new string('a', 32);
            byte[] buffer = Encoding.ASCII.GetBytes(dataBuff);
            
            PingOptions pingOptions = new PingOptions(64, true);
            int timeout = Period * 1000;

            pingSender.SendAsync(Host, timeout, buffer, pingOptions, waiter);
            waiter.WaitOne();
            if (ReplyLog != null) PingCompleted?.Invoke(ReplyLog);
            return true;
        }

        public event DelegatePingCompleted PingCompleted;

        private void PingRoundCompleted(object sender, PingCompletedEventArgs ev)
        {
            if (ev.Cancelled)
            {
                ReplyLog = "Пинг отменён.";
                ((AutoResetEvent) ev.UserState).Set();
            }

            if (ev.Error != null)
            {
                ReplyLog = string.Format("Ошибка пинга: {0}", ev.Error);
                ((AutoResetEvent) ev.UserState).Set();
            }

            PingReply pingReply = ev.Reply;
            ReplyLog = string.Format("{0} {1} {2}", DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss"), Host,
                pingReply.Status);
            ((AutoResetEvent)ev.UserState).Set();
        }
    }
}
