using System;
using System.Collections.Generic;
using AppPinger;
using AppPinger.Protocols;
using AppPinger.Protocols.Implements;
using NUnit.Framework;

namespace NUnitPingerTests.Protocols
{
    [TestFixture]
    class HttpTests
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
        public void Http_for_Constructor_onNullParam()
        {
            Assert.Throws<ArgumentNullException>(() => new HTTP(null));
        }

        [Test]
        public void Http_for_Constructor_onSuccess()
        {
            Assert.DoesNotThrow(() => new HTTP(_configProtocol));
        }

        [Test]
        public void Http_for_StartPing_onSuccess()
        {
            Assert.DoesNotThrow(() =>
            {
                var http = new HTTP(_configProtocol);
                http.StartAsyncPing();
            });
        }

        [Test]
        public void Http_for_StartPing_onEmptyConfigProtocol()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var configProtocol = new ConfigProtocol();
                configProtocol.AttributesProtocol = new Dictionary<string, string>();
                var http = new HTTP(configProtocol);
                http.StartAsyncPing();
            });
        }
    }
}
