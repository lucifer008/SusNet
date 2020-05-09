using SusNet.Services.Da.Domain;
using SusNet.Services.Da.SusDapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet.Services.Service.Impl
{
    public class BridgeServiceImpl : IBridgeService
    {
        public bool IsBridgeStating(int mainTrackNumber, int statingNo, ref BridgeSet bridgeSe)
        {
            var sql = string.Format("select * from BridgeSet Where enabled=@enabled and ( (AMainTrackNumber=@AMainTrackNumber and ASiteNo=@ASiteNo))");
            var bridge = DapperHelp.QueryForObject<BridgeSet>(sql, new
            {
                enabled = 1,
                AMainTrackNumber = mainTrackNumber,
                ASiteNo = statingNo,
                BMainTrackNumber = mainTrackNumber,
                BSiteNo = statingNo
            });
            if (null == bridge)
            {
                sql = string.Format("select * from BridgeSet Where enabled=@enabled and ( (BMainTrackNumber=@BMainTrackNumber and BSiteNo=@BSiteNo))");
                var bridge2 = DapperHelp.QueryForObject<BridgeSet>(sql, new
                {
                    enabled = 1,
                    AMainTrackNumber = mainTrackNumber,
                    ASiteNo = statingNo,
                    BMainTrackNumber = mainTrackNumber,
                    BSiteNo = statingNo
                });
                if (null == bridge2) return false;
                bridgeSe = new BridgeSet();
                bridgeSe.BMainTrackNumber = bridge2.AMainTrackNumber;
                bridgeSe.BSiteNo = bridge2.ASiteNo;
                return true;
            }
            bridgeSe = new BridgeSet();
            bridgeSe.BMainTrackNumber = bridge.BMainTrackNumber;
            bridgeSe.BSiteNo = bridge.BSiteNo;

            return true;
        }
    }
}
