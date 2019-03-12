using System;
using System.Collections.Generic;
using AppPinger;
using NUnit.Framework;

namespace NUnitPingerTests
{
    [TestFixture]
    class SaveLogsTests
    {
        private SaveLogs _saveLogs;

        [SetUp]
        public void Setup()
        {
            var pathDistStorage = new Dictionary<string, string>();
            pathDistStorage.Add("testGlobalPath1", "c:/test1.txt");
            pathDistStorage.Add("testGlobalPath2", "c:/test2.db");
            pathDistStorage.Add("globalDistStorageSqLite", "./test3.db");
            pathDistStorage.Add("globalDistStorage", "./test4.txt");
            _saveLogs = new SaveLogs(pathDistStorage);
        }

        [Test]
        public void SaveLogsTests_for_Constructor()
        {
            Assert.Throws<ArgumentNullException>(() => new SaveLogs(null));
        }

        [Test]
        public void SaveLogsTests_WriteLogAsyncToFile_onSuccessSaveToFile()
        {
            Assert.DoesNotThrow(() => _saveLogs.WriteLogAsyncToFile("Hello World!", "./TestLog.txt"));
        }

        [Test]
        public void SaveLogsTests_WriteLogAsyncToFile_onSuccessSaveToFile_withEmptySecondParam()
        {
            Assert.DoesNotThrow(() => _saveLogs.WriteLogAsyncToFile("Hello World!"));
        }

        [Test]
        public void SaveLogsTests_for_func_GetGlobalDistStorage_onEmptyParam()
        {
            Assert.Throws<NullReferenceException>(() => _saveLogs.GetGlobalDistStorage(""));
        }

        [Test]
        public void SaveLogsTests_for_func_GetGlobalDistStorage()
        {
            Assert.AreEqual(_saveLogs.GetGlobalDistStorage("testGlobalPath1"), "c:/test1.txt");
        }

        [Test]
        public void SaveLogsTests_for_func_CheckIsNullOrEmptyDistStorage_onEmptyParam()
        {
            Assert.Throws<ArgumentNullException>(() => _saveLogs.CheckIsNullOrEmptyDistStorage("",""));
        }

        [Test]
        public void SaveLogsTests_for_func_CheckIsNullOrEmptyDistStorage_onEmptyFirstParam()
        {
            Assert.AreEqual(_saveLogs.CheckIsNullOrEmptyDistStorage("", "TestString"), "TestString");
        }

        [Test]
        public void SaveLogsTests_for_func_CheckIsNullOrEmptyDistStorage_onEmptySecondParam()
        {
            Assert.AreEqual(_saveLogs.CheckIsNullOrEmptyDistStorage("TestString", ""), "TestString");
        }

        [Test]
        public void SaveLogsTests_for_func_CheckIsNullOrEmptyDistStorage_onNotEmptyParams()
        {
            Assert.AreEqual(_saveLogs.CheckIsNullOrEmptyDistStorage("TestString", "TestString2"), "TestString");
        }

        [Test]
        public void SaveLogsTests_WriteLogAsyncToSqLite_onSuccessSaveToDb()
        {
            Assert.DoesNotThrow(
                () => _saveLogs.WriteLogAsyncToSqLite("TestNameProtocol", "Hello World!", "./TestLogDb.db"));
        }

        [Test]
        public void SaveLogsTests_WriteLogAsyncToSqLite_onSuccessSaveToDb_withThirdEmptyParam()
        {
            Assert.DoesNotThrow(
                () => _saveLogs.WriteLogAsyncToSqLite("TestNameProtocol", "Hello World!"));
        }

        [Test]
        public void SaveLogsTests_WriteLogAsyncToSqLite_onSuccessSaveToDb_withEmptyParam()
        {
            Assert.DoesNotThrow(
                () => _saveLogs.WriteLogAsyncToSqLite("", ""));
        }

        [Test]
        public void SaveLogsTests_ViewLogFromSqLite_onSuccessViewFromDb_withEmptyParam()
        {
            Assert.DoesNotThrow(() => _saveLogs.ViewLogFromSqLite());
            Assert.AreNotEqual(_saveLogs.ViewLogFromSqLite(), false);
        }

        [Test]
        public void SaveLogsTests_ViewLogFromSqLite_onSuccessViewFromDb_withParam()
        {
            Assert.DoesNotThrow(() => _saveLogs.ViewLogFromSqLite("./TestLogDb.db"));
            Assert.AreNotEqual(_saveLogs.ViewLogFromSqLite("./TestLogDb.db"), false);
        }
    }
}
