namespace AppPinger.Protocols.Interfaces
{
    public interface IConfigProtocol
    {
        string Host { get; set; }
        int Period { get; set; }
        string NameProt { get; set; }
        string AdditionalAttribute { get; set; }
    }
}
