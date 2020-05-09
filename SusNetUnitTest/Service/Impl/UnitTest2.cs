using Microsoft.VisualStudio.TestTools.UnitTesting;
using SusNet.Services.Service.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet.Services.Service.Impl.Tests
{
    [TestClass()]
    public class UnitTest2
    {
        [TestMethod()]
        public void IsBridgeStatingTest()
        {
            Da.Domain.BridgeSet bridgeSet = null;
            var bridgeService = new BridgeServiceImpl().IsBridgeStating(1, 110,ref bridgeSet);
            Console.WriteLine(bridgeService);
        }
    }
}