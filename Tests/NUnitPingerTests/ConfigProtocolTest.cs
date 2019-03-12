using System;
using System.Collections.Generic;
using AppPinger.Protocols;
using NUnit.Framework;

namespace NUnitPingerTests
{
    [TestFixture()]
    class ConfigProtocolTest
    {
        private ConfigProtocol _configProtocol;

        [SetUp]
        public void Setup()
        {
            _configProtocol = new ConfigProtocol();

        }

        [Test]
        public void ConfigProtocol_for_func_GetAdditionalAttribute_onNotExistContainerAttributes()
        {
            Assert.Throws<NullReferenceException>(() => _configProtocol.GetAdditionalAttribute("attributeNotExist"));
        }

        [Test]
        public void ConfigProtocol_for_func_GetAdditionalAttribute_onEmptyNameAttribute()
        {
            Assert.Throws<NullReferenceException>(() => _configProtocol.GetAdditionalAttribute(""));
        }

        [Test]
        public void ConfigProtocol_for_func_GetAdditionalAttribute_onNotExistAttribute()
        {
            _configProtocol.AttributesProtocol = new Dictionary<string, string>();
            _configProtocol.AttributesProtocol.Add("attributeExist", "test");
            Assert.IsNull(_configProtocol.GetAdditionalAttribute("attributeNotExist"));
        }

        [Test]
        public void ConfigProtocol_for_func_GetAdditionalAttribute_onExistAttribute()
        {
            _configProtocol.AttributesProtocol = new Dictionary<string, string>();
            _configProtocol.AttributesProtocol.Add("attributeExist", "test");
            Assert.IsNotNull(_configProtocol.GetAdditionalAttribute("attributeExist"));
        }
    }
}
