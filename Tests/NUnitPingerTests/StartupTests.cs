using System;
using System.IO;
using AppPinger;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace NUnitPingerTests
{
    [TestFixture]
    class StartupTests
    {
        private IConfiguration _appConfig;

        [SetUp]
        public void Setup()
        {
            string pathAppConfig = "appConfig.json";
            _appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(pathAppConfig, optional: true, reloadOnChange: true)
                .Build();
        }

        [Test]
        public void StartupClassTest_for_func_InitPinger_onNullArg()
        {
            Assert.Throws<NullReferenceException>(() => Startup.InitPinger(null));
        }

        [Test]
        public void StartupClassTest_for_func_InitPinger_onSuccess()
        {
            Assert.DoesNotThrow(() => Startup.InitPinger(_appConfig));
        }
    }
}
