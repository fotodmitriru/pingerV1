using System;
using System.Collections.Generic;

namespace AppPinger.Protocols
{
    public class ConfigProtocol
    {
        public string NameProt { get; set; }
        public List<string> HeadersAddAttr { get; set; }
        public Object[] AdditionalAttributes { get; set; }

        public object GetAdditionalAttribute(string nameAttribute)
        {
            return AdditionalAttributes[HeadersAddAttr.IndexOf(nameAttribute)];
        }
    }
}
