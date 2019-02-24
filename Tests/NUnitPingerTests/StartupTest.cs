using System;
using AppPinger;
using NUnit.Framework;

namespace NUnitPingerTests
{
    [TestFixture]
    class StartupTest
    {
        [Test]
        public void StartupClassTest_for_func_InitPinger_onNullArg()
        {
            Assert.Throws<NullReferenceException>(() => Startup.InitPinger(null));
        }
    }
}
