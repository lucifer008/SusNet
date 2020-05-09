using Sus.Net.Common.Message;
using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.SusBusMessage
{
    public class ClearHangerCacheRequestMessage: MessageBody
    {
        private const string cmd = "03";
        private const string addh = "00";
        private const string addl = "52";

        //public static ClearHangerCacheRequestMessage isEqual(Byte[] resBytes)
        //{
        //    var rCmd = Utils.HexHelper.byteToHexStr(new byte[] { resBytes[2] });
        //    if (!cmd.Equals(rCmd)) return null;

        //    var rAddl = HexHelper.byteToHexStr(new byte[] { resBytes[4] });
        //    if (!addh.Equals(rAddl)) return null;

        //    var rAddh = HexHelper.byteToHexStr(new byte[] { resBytes[5] });
        //    if (!addl.Equals(rAddl)) return null;

        //    IList<byte> bList = new List<byte>();
        //    for (var index = 7; index < 12; index++)
        //    {
        //        bList.Add(resBytes[index]);
        //    }
        //    return new ClearHangerCacheRequestMessage(resBytes)
        //    {
        //        HangerNo = HexHelper.HexToTen(HexHelper.bytesToHexString(bList.ToArray()))
        //    };
        //}
        public int HangerNo { set; get; }
    }
}
