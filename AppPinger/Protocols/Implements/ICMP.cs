using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using AppPinger.Protocols.Interfaces;

namespace AppPinger.Protocols.Implements
{
    class ICMP: IICMP
    {
        public string Host { get; set; }
        public int Period { get; set; }
        public string ReplyLog { get; set; }

        public ICMP(IConfigProtocol confProtocols)
        {
            if (confProtocols == null)
                throw new ArgumentException("Не заданы параметры для протокола ICMP!");
            Host = confProtocols.Host ?? throw new NullReferenceException("Параметр Host не задан!");
            Period = confProtocols.Period;
        }

        public bool StartPing()
        {
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
            Console.WriteLine(ReplyLog);
            return true;
        }

        private void PingRoundCompleted(object sender, PingCompletedEventArgs ev)
        {
            if (ev.Cancelled)
            {
                Console.WriteLine("Пинг отменён.");
                ((AutoResetEvent)ev.UserState).Set();
            }

            if (ev.Error != null)
            {
                Console.WriteLine("Ошибка пинга:");
                Console.WriteLine(ev.Error.ToString());
                ((AutoResetEvent)ev.UserState).Set();
            }

            PingReply pingReply = ev.Reply;
            ReplyLog = string.Format("{0} {1} {2}", DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss"), Host,
                pingReply.Status);
            SaveLogs.WriteLog("C:\\Logs.txt", ReplyLog);     //path???
            ((AutoResetEvent)ev.UserState).Set();
        }
    }
}
