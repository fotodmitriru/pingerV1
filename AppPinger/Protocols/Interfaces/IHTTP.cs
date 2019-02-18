namespace AppPinger.Protocols.Interfaces
{
    interface IHTTP : IBasePingProtocol
    {
        int ValidCode { get; set; }
    }
}
