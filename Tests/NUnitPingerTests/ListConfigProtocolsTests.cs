using System;
using System.IO;
using AppPinger.Protocols.Implements;
using NUnit.Framework;

namespace NUnitPingerTests
{
    [TestFixture]
    class ListConfigProtocolsTests
    {
        private ListConfigProtocols _listConfigProtocols;
        private string _pathAppConfig;

        [SetUp]
        public void Setup()
        {
            _listConfigProtocols = new ListConfigProtocols();
            _pathAppConfig = Directory.GetCurrentDirectory() +
                            $@"\..\..\..\..\..\AppPinger\bin\Debug\netcoreapp2.1\";
        }

        [Test]
        public void ListConfigProtocols_for_func_ReadConfig()
        {
            Assert.IsTrue(_listConfigProtocols.ReadConfig(_pathAppConfig + "listHosts.json"));
        }

        [Test]
        public void ListConfigProtocols_for_func_ReadConfig_onExistsFileFalse()
        {
            Assert.IsFalse(_listConfigProtocols.ReadConfig(_pathAppConfig + "lis.json"));
        }

        [Test]
        public void ListConfigProtocols_for_func_ReadConfig_onFileFailedStructure()
        {
            Assert.Throws<NullReferenceException>(() =>
                _listConfigProtocols.ReadConfig(_pathAppConfig + "appConfig.json"));
        }
    }
}
