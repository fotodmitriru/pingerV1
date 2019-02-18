namespace AppPinger.Protocols.Interfaces
{
    public interface IBasePingProtocol
    {
        string Host { get; set; }
        int Period { get; set; }
        string ReplyLog { get; set; }
        bool StartPing(IConfigProtocol confProtocol);
        event DelegatePingCompleted PingCompleted;
    }

    public delegate void DelegatePingCompleted(string replyLog);
}
