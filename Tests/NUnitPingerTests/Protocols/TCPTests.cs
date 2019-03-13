using System;
using System.Collections.Generic;
using AppPinger;
using AppPinger.Protocols;
using AppPinger.Protocols.Implements;
using NUnit.Framework;

namespace NUnitPingerTests.Protocols
{
    [TestFixture]
    class TcpTests
    {
        private ConfigProtocol _configProtocol;

        [SetUp]
        public void SetUp()
        {
            _configProtocol = new ConfigProtocol()
            {
                NameProt = EnumProtocols.Tcp,
                Host = "ya.ru",
                Period = 1,
                AttributesProtocol = new Dictionary<string, string>()
            };
            _configProtocol.AttributesProtocol.Add("Port", "80");
        }

        [Test]
        public void Tcp_for_Constructor_onNullParam()
        {
            Assert.Throws<ArgumentNullException>(() => new TCP(null));
        }

        [Test]
        public void Tcp_for_Constructor_onSuccess()
        {
            Assert.DoesNotThrow(() => new TCP(_configProtocol));
        }

        [Test]
        public void Tcp_for_StartPing_onSuccess()
        {
            Assert.DoesNotThrow(() =>
            {
                var tcp = new TCP(_configProtocol);
                tcp.StartAsyncPing();
            });
        }

        [Test]
        public void Tcp_for_StartPing_onEmptyConfigProtocol()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var configProtocol = new ConfigProtocol();
                configProtocol.AttributesProtocol = new Dictionary<string, string>();
                var tcp = new TCP(configProtocol);
                tcp.StartAsyncPing();
            });
        }
    }
}
