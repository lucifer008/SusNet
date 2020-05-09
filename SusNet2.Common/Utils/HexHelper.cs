using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.Utils
{
    public class HexHelper
    {

        /// <summary>
        /// 十进制转16进制字符串
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string tenToHexString(int v) {
            return v.ToString("X2");
        }

        /// <summary>
        /// 十进制转16进制字符串
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string tenToHexString(string v)
        {
            return int.Parse(v).ToString("X2");
        }
        //// <summary>
        /// 十六进制字符串转字节
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += "0";//hexString += " ";
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
        public static string ToHex(string s, string charset=null, bool fenge=false)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                         //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.UnicodeEncoding.Unicode;// System.Text.UnicodeEncoding.GetEncoding(charset);
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
        /// <summary>
        /// 十进制转16进制字符串:(只针对一个字节的整数)
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string TenToHexString2Len(string v)
        {
            return int.Parse(v).ToString("X2");
        }
        public static string TenToHexString2Len(int v)
        {
            return v.ToString("X2");
        }
        /// <summary>
        /// 十进制转16进制字符串(2字节长度，不足补0)
        /// 兼容有一定的问题:这样的数 0160
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string TenToHexString4Len(int v)
        {
            return v.ToString("X4");
        }

        public static int HexToTen(string hexStr) {
            return Int32.Parse(hexStr, System.Globalization.NumberStyles.HexNumber);
        }
        ///<summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }
        /// <summary>
        /// 将string格式转化为十六进制数据
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <returns>
        /// </returns>
        public static string tenToSixteen(string msg)
        {
            long number = Convert.ToInt64(msg);
            return Convert.ToString(number, 16);
        }
        /// <summary>
        /// 十进制转16进制字符串(5字节长度，不足补0)
        /// 兼容有一定的问题:这样的数 0160
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string TenToHexString10Len(string v)
        {
            return int.Parse(v).ToString("X10");
        }

        /// <summary>
        /// 十进制转16进制字符串(5字节长度，不足补0)
        /// 兼容有一定的问题:这样的数 0160
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string TenToHexString10Len(int v)
        {
            return v.ToString("X10");
        }
        /// <summary>
        /// 获取BCC校验码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte GetBCCNumer(byte[] bytes)
        {
            byte byteBCC;
            byteBCC = (byte)(bytes[0] ^ bytes[1]);
            for (int i = 2; i < bytes.Length; i++)
            {
                if (i == 3)
                    continue;

                byteBCC = (byte)(byteBCC ^ bytes[i]);
            }

            return byteBCC;
        }

    }
}
