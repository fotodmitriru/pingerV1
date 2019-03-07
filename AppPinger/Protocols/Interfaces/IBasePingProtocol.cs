namespace AppPinger.Protocols.Interfaces
{
    public interface IBasePingProtocol
    {
        bool StartAsyncPing();
        event DelegatePingCompleted PingCompleted;
    }
}
