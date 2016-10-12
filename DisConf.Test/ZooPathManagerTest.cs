using System;
using DisConf.Utility.Path;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DisConf.Test
{
    [TestClass]
    public class ZooPathManagerTest
    {
        [TestMethod]
        public void TryGetInfo()
        {
            string app, env, name;
            Assert.IsTrue(ZooPathManager.TryGetInfo("/APP1_online/Config1", out app, out env, out name));
        }
    }
}
