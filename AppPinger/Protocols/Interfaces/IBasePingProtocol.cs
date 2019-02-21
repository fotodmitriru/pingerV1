using System.Threading.Tasks;
using AppPinger.Protocols.Interfaces.Implements;

namespace AppPinger.Protocols.Interfaces
{
    public interface IBasePingProtocol
    {
        string Host { get; set; }
        int Period { get; set; }
        bool StartPing(ConfigProtocol confProtocol);
        event DelegatePingCompleted PingCompleted;
    }

    public delegate void DelegatePingCompleted(string replyLog);
}
