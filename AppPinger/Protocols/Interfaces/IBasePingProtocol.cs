namespace AppPinger.Protocols.Interfaces
{
    public interface IBasePingProtocol
    {
        bool StartPing();
        event DelegatePingCompleted PingCompleted;
    }
}
