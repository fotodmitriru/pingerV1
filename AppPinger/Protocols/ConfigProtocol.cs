using System;
using System.Collections.Generic;

namespace AppPinger.Protocols
{
    public class ConfigProtocol
    {
        public EnumProtocols NameProt { get; set; }
        public List<string> HeadersAddAttr { get; set; }
        public Object[] AdditionalAttributes { get; set; }

        public object GetAdditionalAttribute(string nameAttribute)
        {
            if (string.IsNullOrEmpty(nameAttribute))
                throw new NullReferenceException("Укажите имя атрибута!");
            return AdditionalAttributes[HeadersAddAttr.IndexOf(nameAttribute)];
        }
    }
}
