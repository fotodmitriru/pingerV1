using System;
using System.Collections.Generic;
using AppPinger;
using AppPinger.Protocols;
using AppPinger.Protocols.Implements;
using NUnit.Framework;

namespace NUnitPingerTests.Protocols
{
    [TestFixture]
    class IcmpTests
    {
        private ConfigProtocol _configProtocol;

        [SetUp]
        public void SetUp()
        {
            _configProtocol = new ConfigProtocol()
            {
                NameProt = EnumProtocols.Icmp,
                Host = "ya.ru",
                Period = 1,
                AttributesProtocol = new Dictionary<string, string>()
            };
        }

        [Test]
        public void Icmp_for_Constructor_onNullParam()
        {
            Assert.Throws<ArgumentNullException>(() => new ICMP(null));
        }

        [Test]
        public void Icmp_for_Constructor_onSuccess()
        {
            Assert.DoesNotThrow(() => new ICMP(_configProtocol));
        }

        [Test]
        public void Icmp_for_StartPing_onSuccess()
        {
            Assert.DoesNotThrow(() =>
            {
                var icmp = new ICMP(_configProtocol);
                icmp.StartAsyncPing();
            });
        }

        [Test]
        public void Icmp_for_StartPing_onEmptyConfigProtocol()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var configProtocol = new ConfigProtocol();
                configProtocol.AttributesProtocol = new Dictionary<string, string>();
                var icmp = new ICMP(configProtocol);
                icmp.StartAsyncPing();
            });
        }
    }
}
