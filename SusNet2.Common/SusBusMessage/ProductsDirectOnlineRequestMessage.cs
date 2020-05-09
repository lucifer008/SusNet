
using SusNet2.Common.Message;
using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.SusBusMessage
{
    /// <summary>
    /// 【XID+ID+CMD+XOR+ADDH+ADDL+DATA1+DATA2+DATA3+DATA4+DATA5+DATA6】
    /// 制品界面直接上线pc向硬件发送上线信息【 pc--->硬件 】
    /// 
    /// </summary>
    public class ProductsDirectOnlineRequestMessage : MessageBody
    {
        public ProductsDirectOnlineRequestMessage(byte[] resBytes) : base(resBytes)
        {

        }
        /// <summary>
        /// 制品界面直接上线pc向硬件发送上线信息
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
        /// 是否是制品界面直接上线p请求
        /// </summary>
        /// <param name="resBytes"></param>
        /// <returns></returns>
        public static bool isEqual(Byte[] resBytes, out List<byte> productsInfo)
        {
            // Array ar = null;

            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            var cmd = HexHelper.byteToHexStr(new byte[] { resBytes[0], resBytes[1], resBytes[2] });
            if (cmd.Equals("010405"))
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
        /// 是否是制品界面直接上线请求结束
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
            if (cmd.Equals("01 04 05".Replace(" ", "")) && "00 00 00 00 00 00".Replace(" ", "").Equals(endCmdHex))
            {
                return true;
            }
            return false;
        }

    }
}
