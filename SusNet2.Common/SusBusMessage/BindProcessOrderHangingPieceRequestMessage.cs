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
    /// 制单绑定到挂片站完成时请求【 pc--->硬件 】
    /// 
    /// </summary>
    public class BindProcessOrderHangingPieceRequestMessage : MessageBody
    {
        public BindProcessOrderHangingPieceRequestMessage(byte[] resBytes) : base(resBytes)
        {

        }
        /// <summary>
        /// 处理制品界面选择挂片后对pc的消息发送
        /// </summary>
        /// <param name="mainTrackNo">主轨号</param>
        /// <param name="statingNo">挂片站</param>
        /// <param name="addh"></param>
        /// <param name="addl"></param>
        /// <param name="productNumber">排产号</param>
        /// <param name="xor"></param>
        public BindProcessOrderHangingPieceRequestMessage(string mainTrackNo, string statingNo, string addh, string addl, string productNumber, int index, string xor = null)
        {
            XID = mainTrackNo;
            ID = statingNo;
            CMD = "03";
            XOR = "00";
            if (!string.IsNullOrEmpty(xor))
                XOR = xor;
            ADDH = addh;
            ADDL = addl;
            if (index == 0)
            {
                DATA1 = string.Format("{0}", productNumber);

            }

        }
        /// <summary>
        /// 处理制品界面选择挂片后对pc的消息发送
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="addh"></param>
        /// <param name="addl"></param>
        /// <param name="productNumber"></param>
        /// <param name="index"></param>
        /// <param name="xor"></param>
        /// <returns></returns>
        public static byte[] GetHeaderBytes(string mainTrackNo, string statingNo, string addh, string addl, int productNumber, int index, string xor = null)
        {
            if (string.IsNullOrEmpty(xor))
                xor = "00";
            if (index == 0)
            {
                if (productNumber > 255)
                {
                    string hexStrFirst1 = string.Format("{0} {1} {2} {3} {4} {5} {6}", mainTrackNo, statingNo, "03", xor, addh, addl, HexHelper.tenToHexString(productNumber));
                    return HexHelper.strToToHexByte(hexStrFirst1);
                }
                var hexProductNumber =HexHelper.tenToHexString(string.Format("{0:00}", productNumber));
                string hexStrFirst = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", mainTrackNo, statingNo, "03", xor, addh, addl, "00", hexProductNumber);
                return HexHelper.strToToHexByte(hexStrFirst);
            }
            string hexStr = string.Format("{0} {1} {2} {3} {4} {5}", mainTrackNo, statingNo, "03", xor, addh, addl);
            return HexHelper.strToToHexByte(hexStr);
        }

        /// <summary>
        /// 是否是制品界面挂片请求
        /// </summary>
        /// <param name="resBytes"></param>
        /// <returns></returns>
        public static bool isEqual(Byte[] resBytes, out List<byte> products,out List<byte> productNumber)
        {
            // Array ar = null;

            // var hexStr = HexHelper.byteToHexStr(bList.ToArray());
            var cmd = HexHelper.byteToHexStr(new byte[] { resBytes[0], resBytes[1], resBytes[2] });
            if (cmd.Equals("887703"))
            {
                products = new List<byte>();
                productNumber = new List<byte>();
                for (int b = 6; b < resBytes.Length; b++)
                {
                    if ("00" == HexHelper.byteToHexStr(new byte[] { resBytes[5] }))//是否是一个制品的第一条命令【01 01 03 XX 60 00 00 08 xx xx xx xx】
                    {
                        var jj = b;
                        if (b < resBytes.Length - 1 && jj != 6)
                        {
                            jj = b + 1;
                        }
                        else if (b < resBytes.Length - 1 && jj == 6) {
                            jj = b + 2;//隔2字节
                            productNumber.Add(resBytes[b]);
                            productNumber.Add(resBytes[b + 1]);
                        }
                        if (!"FF".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[jj] })))
                        {
                            products.Add(resBytes[jj]);
                        }
                    }
                    else
                    {
                        if (!"FF".Equals(HexHelper.byteToHexStr(new byte[] { resBytes[b] })))
                        {
                            products.Add(resBytes[b]);
                        }
                    }
                }
                //productNumber = null;
                return true;
            }
            products = null;
            productNumber = null;
            return false;
        }
        /// <summary>
        /// 是否是制品界面挂片请求结束
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
            for (var i = 5; i < resBytes.Length; i++)
            {
                list.Add(resBytes[i]);
            }
            var endCmdHex = HexHelper.byteToHexStr(list.ToArray());
            IList<byte> bList = new List<byte>();
            if (cmd.Equals("88 77 03".Replace(" ", "")) && "0F FF FF FF FF FF FF".Replace(" ", "").Equals(endCmdHex))
            {
                return true;
            }
            return false;
        }
        public string ProductNumber { set; get; }
    }
}
