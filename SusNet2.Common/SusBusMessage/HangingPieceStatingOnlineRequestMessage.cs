using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.SusBusMessage
{

    /// <summary>
    /// 挂片站终端上线请求
    /// </summary>
   public class HangingPieceStatingOnlineRequestMessage : SusNet2.Common.Message.MessageBody
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainTrackNo">主轨号</param>
        /// <param name="statingNo">挂片站</param>
        /// <param name="addh"></param>
        /// <param name="addl"></param>
        /// <param name="productNumber">排产号</param>
        /// <param name="xor"></param>
        public HangingPieceStatingOnlineRequestMessage(string mainTrackNo, string statingNo, string addh, string addl, string productNumber, string xor = null)
        {
            XID = mainTrackNo;
            ID = statingNo;
            CMD = "03";
            XOR = "00";
            if (!string.IsNullOrEmpty(xor))
                XOR = xor;
            ADDH = addh;
            ADDL = addl;
            DATA1 = "00";
            DATA2 = "00";
            DATA3 = "00";
            DATA4 = "00";
            DATA5 = "00";
            DATA6 = productNumber;
        }
        public HangingPieceStatingOnlineRequestMessage(byte[] resBytes) : base(resBytes)
        {

        }
        public static HangingPieceStatingOnlineRequestMessage isEqual(Byte[] resBytes)
        {
            // Array ar = null;

            //ProductNumber = HexHelper.byteToHexStr(bList.ToArray());

            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            if ("06".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[2] })) && "00".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[4] })) && "35".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[5] })))
            {
                IList<byte> bList = new List<byte>();
                bList.Add(resBytes[11]);
                return new HangingPieceStatingOnlineRequestMessage(resBytes)
                {
                    ProductNumber = HexHelper.byteToHexStr(bList.ToArray())
                };
            }
            return null;
        }
        public string ProductNumber { set; get; }
    }
}
