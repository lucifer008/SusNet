using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.SusBusMessage
{
    /// <summary>
    /// 挂片站终端上线pc回应
    /// </summary>
    public class HangingPieceStatingOnlineResponseMessage : SusNet2.Common.Message.MessageBody
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
        public HangingPieceStatingOnlineResponseMessage(string mainTrackNo, string statingNo, string addh, string addl, string productNumber, string xor = null)
        {
            XID = mainTrackNo;
            ID = statingNo;
            CMD = "05";
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
        public HangingPieceStatingOnlineResponseMessage(byte[] resBytes) : base(resBytes)
        {

        }
        /// <summary>
        /// 【挂片站终端上线pc回应  (上线信息回写到硬件)  】pc向硬件发送上线信息
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="addh"></param>
        /// <param name="addl"></param>
        /// <param name="productNumber"></param>
        /// <param name="index"></param>
        /// <param name="xor"></param>
        /// <returns></returns>
        public static byte[] GetHeaderBytes(string mainTrackNo, string statingNo, string addh, string addl, int index, string xor = null)
        {
            if (string.IsNullOrEmpty(xor))
                xor = "00";
            string hexStr = string.Format("{0} {1} {2} {3} {4} {5}", mainTrackNo, statingNo, "05", xor, addh, addl);
            return HexHelper.strToToHexByte(hexStr);
        }
        /// <summary>
        /// 是否是【挂片站终端上线pc回应  (上线信息回写到硬件)  】p请求
        /// </summary>
        /// <param name="resBytes"></param>
        /// <returns></returns>
        public static bool isEqual(Byte[] resBytes, out List<byte> productsInfo)
        {
            // Array ar = null;

            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            var cmd = HexHelper.byteToHexStr(new byte[] { resBytes[0], resBytes[1], resBytes[2] });
            if (cmd.Equals("887805"))
            {
                productsInfo = new List<byte>();

                for (int b = 6; b < resBytes.Length; b++)
                {
                    productsInfo.Add(resBytes[b]);
                }
                //productNumber = null;
                return true;
            }
            productsInfo = null;
            return false;
        }
        /// <summary>
        /// 是否是【挂片站终端上线pc回应 (上线信息回写到硬件)  】请求结束
        /// </summary>
        /// <param name="resBytes"></param>
        /// <returns></returns>
        public static bool isEnd(Byte[] resBytes)
        {
            // Array ar = null;

            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            var cmd = HexHelper.byteToHexStr(new byte[] { resBytes[0], resBytes[1], resBytes[2] });
            //var endFlag = new byte[7];
            //resBytes.CopyTo(endFlag, 5);
            var list = new List<byte>();
            for (var i = 6; i < resBytes.Length; i++)
            {
                list.Add(resBytes[i]);
            }
            var endCmdHex = HexHelper.byteToHexStr(list.ToArray());
            IList<byte> bList = new List<byte>();
            if (cmd.Equals("88 78 05".Replace(" ", "")) && "00 00 00 00 00 00".Replace(" ", "").Equals(endCmdHex))
            {
                return true;
            }
            return false;
        }

        public string ProductNumber { set; get; }
    }
}
