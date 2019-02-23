using System.Threading.Tasks;
using AppPinger.Protocols.Interfaces.Implements;

namespace AppPinger.Protocols.Interfaces
{
    public interface IBasePingProtocol
    {
        string Host { get; set; }
        int Period { get; set; }
        string DistStorage { get; set; }
        bool StartPing();
        event DelegatePingCompleted PingCompleted;
    }
}
