using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SusNet.Services.Da.SusDapper;
using SusNet.Services.Da.Domain;

namespace SusNetUnitTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            var sql = string.Format("select * from BridgeSet Where enabled=@enabled");
            var list = DapperHelp.QueryForList<BridgeSet>(sql,new { enabled =1});
            Console.WriteLine(list.Count);
        }
    }
}
