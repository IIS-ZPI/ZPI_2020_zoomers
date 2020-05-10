using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace zpi_aspnet_test.Tests.Config
{
    [TestClass]
    class ConfigTest
    {
        [TestMethod]
        public void Config()
        {
            Assert.IsNotNull(ConfigurationManager.ConnectionStrings);

            Assert.IsTrue(ConfigurationManager.ConnectionStrings.Count >= 2);
        } 
    }
}
