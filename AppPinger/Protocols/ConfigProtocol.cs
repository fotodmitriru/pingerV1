using System;
using System.Collections.Generic;

namespace AppPinger.Protocols
{
    public class ConfigProtocol
    {
        public EnumProtocols NameProt { get; set; }
        public string Host { get; set; }
        public int Period { get; set; }
        public Dictionary<string, string> AttributesProtocol { get; set; }

        public object GetAdditionalAttribute(string nameAttribute)
        {
            if (string.IsNullOrEmpty(nameAttribute))
                throw new NullReferenceException("Укажите имя атрибута!");
            if (AttributesProtocol == null)
                throw new NullReferenceException("Не создан контейнер с атрибутами!");

            return (AttributesProtocol.ContainsKey(nameAttribute)) ? AttributesProtocol[nameAttribute] : null;
        }
    }
}
