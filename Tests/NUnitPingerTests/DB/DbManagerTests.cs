using System;
using AppPinger.DB;
using NUnit.Framework;

namespace NUnitPingerTests.DB
{
    [TestFixture]
    class DbManagerTests
    {
        private string _nameProtocol = "TestName";
        private string _dataLog = "TestLog";
        private string _dbConnectionString = "Data Source=./Test.db";

        [Test]
        public void DbManager_WriteToDbAsync_onEmptyParams()
        {
            var dbManager = new DbManager();
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await dbManager.WriteToDbAsync("", "", _dbConnectionString, EnumProviderDb.SqLite);
            });
        }

        [Test]
        public void DbManager_WriteToDbAsync_onSuccess()
        {
            var dbManager = new DbManager();
            Assert.DoesNotThrowAsync(async () =>
            {
                await dbManager.WriteToDbAsync(_nameProtocol, _dataLog, _dbConnectionString, EnumProviderDb.SqLite);
            });
        }

        [Test]
        public void DbManager_ViewDb_onEmptyParams()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var dbManager = new DbManager();
                dbManager.ViewDb("", EnumProviderDb.SqLite);
            });
        }

        [Test]
        public void DbManager_ViewDb_onSuccess()
        {
            Assert.DoesNotThrow(() =>
            {
                var dbManager = new DbManager();
                dbManager.ViewDb(_dbConnectionString, EnumProviderDb.SqLite);
            });
        }
    }
}
