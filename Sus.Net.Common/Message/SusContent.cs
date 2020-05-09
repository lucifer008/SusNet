using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sus.Net.Common.Message
{
    /// <summary>
    /// Sus 硬件消息协议结构
    /// </summary>
    public class SusContent
    {
        public string XID = "00";
        public string ID = "01";//站ID
        //public UInt16 CMD = 01;
        public CMD_TYPE CMD_TYPE;
        public string ADDH = "0001";
        public string ADDL = "0001";
        public string DATA1;
        public string DATA2;
        public string DATA3;
        public string DATA4;
        public string DATA5;
        public string DATA6;
        public override string ToString()
        {
            this.CMD_TYPE = CMD_TYPE.PC_QUERY;

            var xid = strToToHexByte(XID);
            var id = strToToHexByte(ID);
            var addh = strToToHexByte(ADDH);
            var addl = strToToHexByte(ADDL);
            var byteList = new List<byte>();
            byteList.AddRange(xid);
            byteList.AddRange(id);
            byteList.AddRange(addh);
            byteList.AddRange(addl);

            var v = byteToHexStr(byteList.ToArray());
            return v;
            //return (XID.ToString()+ID.ToString() + this.CMD_TYPE.ToString() + ADDH.ToString() + ADDL.ToString() + DATA1.ToString()).ToString();
        }
        public byte[] GetContent() {
            this.CMD_TYPE = CMD_TYPE.PC_QUERY;

            var xid = strToToHexByte(XID);
            var id = strToToHexByte(ID);
            var addh = strToToHexByte(ADDH);
            var addl = strToToHexByte(ADDL);
            var byteList = new List<byte>();
            byteList.AddRange(xid);
            byteList.AddRange(id);
            byteList.AddRange(addh);
            byteList.AddRange(addl);
            return byteList.ToArray();
        }
        //https://www.cnblogs.com/seventeen/archive/2009/10/29/1591936.html
        //// <summary>
        /// 十六进制字符串转字节
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 从汉字转换到16进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <param name="fenge">是否每字符用逗号分隔</param>
        /// <returns></returns>
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                         //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }
    }
}
