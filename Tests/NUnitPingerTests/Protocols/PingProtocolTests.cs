using System;
using System.Collections.Generic;
using System.IO;
using AppPinger;
using AppPinger.Protocols;
using AppPinger.Protocols.Implements;
using AppPinger.Protocols.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace NUnitPingerTests.Protocols
{
    [TestFixture()]
    class PingProtocolTests
    {
        private IApplicationBuilder _appBuilder;
        private IServiceCollection _serviceCollection;
        private IListConfigProtocols _listConfigProtocols;

        [SetUp]
        public void SetUp()
        {
            IConfiguration appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appConfig.json", optional: true, reloadOnChange: true)
                .Build();
            _serviceCollection = new ServiceCollection();

            ConfigurePinger(_serviceCollection, appConfig);

            var serviceProvider = _serviceCollection.BuildServiceProvider();
            _appBuilder = new ApplicationBuilder(serviceProvider);
            _listConfigProtocols = _appBuilder.ApplicationServices.GetService<IListConfigProtocols>();
            _listConfigProtocols.ReadConfig(appConfig["listHosts"]);
        }

        [Test]
        public void PingProtocols_for_Constructor_onEmptyFirstParam()
        {
            Assert.Throws<NullReferenceException>(() =>
                new PingProtocols(null, _serviceCollection, _listConfigProtocols));
        }

        [Test]
        public void PingProtocols_for_Constructor_onEmptySecondParam()
        {
            Assert.Throws<NullReferenceException>(() => new PingProtocols(_appBuilder, null, _listConfigProtocols));
        }

        [Test]
        public void PingProtocols_for_Constructor_onSuccess()
        {
            Assert.DoesNotThrow(() => new PingProtocols(_appBuilder, _serviceCollection, _listConfigProtocols));
        }

        [Test]
        public void PingProtocols_for_StartPing_onNotReadHosts()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                var pingProtocols = new PingProtocols(_appBuilder, _serviceCollection, new ListConfigProtocols());
                pingProtocols.StartPing();
            });
        }

        [Test]
        public void PingProtocols_for_StartPing_onSuccess()
        {
            Assert.DoesNotThrow(() =>
            {
                var pingProtocols = new PingProtocols(_appBuilder, _serviceCollection, _listConfigProtocols);
                pingProtocols.StartPing();
            });
        }

        private static void ConfigurePinger(IServiceCollection serviceCollection, IConfiguration appConfig)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddTransient<ConfigProtocol>();
            serviceCollection.AddSingleton<IListConfigProtocols, ListConfigProtocols>();

            var globalDistStorage = new Dictionary<string, string>();
            globalDistStorage.Add("globalDistStorage", appConfig["fileLogs"]);
            globalDistStorage.Add("globalDistStorageSqLite", appConfig["fileLogsSQLite"]);
            serviceCollection.AddSingleton(x =>
                ActivatorUtilities.CreateInstance<SaveLogs>(x, globalDistStorage));
        }
    }
}