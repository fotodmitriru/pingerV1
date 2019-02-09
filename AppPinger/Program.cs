using System;
using AppPinger.Protocols.Implements;

namespace AppPinger
{
    class Program
    {
        static void Main(string[] args)
        {
            var listConfig = new ListConfigProtocols();
            if (listConfig.ReadConfig("./listHosts.xml"))
            {
                Console.WriteLine("Hosts read!");
            }
            var icmp = new ICMP(listConfig.ListConfProtocols[0]);

            Console.WriteLine(icmp.Host);
            icmp.StartPing();
            Console.ReadLine();
        }
    }
}
