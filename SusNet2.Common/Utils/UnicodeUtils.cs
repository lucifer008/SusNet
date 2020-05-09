using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.Utils
{
    public class UnicodeUtils
    {
        /// <summary>
        /// unicode编码
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static byte[] GetBytesByUnicode(String values)
        {
            return System.Text.UnicodeEncoding.Unicode.GetBytes(values);
        }
        /// <summary>
        /// 对unicode编码的解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string StringByDecoder(byte[] data) {
            var decoder = System.Text.UnicodeEncoding.Unicode.GetDecoder();
            int charSize = System.Text.UnicodeEncoding.Unicode.GetDecoder().GetCharCount(data, 0, data.Length);
            Char[] chs = new char[charSize];
            //进行字符转换;
            int charLength = decoder.GetChars(data, 0, data.Length, chs, 0);
            //Console.WriteLine(new String(chs));
            return new String(chs);
        }
        /// <summary>
        /// 从汉字转换到16进制
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetHexFromChs(string s)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                         //throw new ArgumentException("s is not valid chinese string!");
            }

            System.Text.Encoding chs = System.Text.UnicodeEncoding.GetEncoding("gb2312");

            byte[] bytes = chs.GetBytes(s);

            string str = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
            }

            return str;
        }
        /// <summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string GetChsFromHex(string hex)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
                            //throw new ArgumentException("hex is not a valid number!", "hex");
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

            // 获得 GB2312，Chinese Simplified。
            System.Text.Encoding chs = System.Text.UnicodeEncoding.GetEncoding("gb2312");


            return chs.GetString(bytes);
        }
        ///// <summary>
        ///// 对【Unicode编码】的字符串解码
        ///// </summary>
        ///// <returns>The to unicode.</returns>
        ///// <param name="value">Value.</param>
        //public static string StringToUnicode(string value)
        //{
        //    byte[] bytes = Encoding.Unicode.GetBytes(value);
        //    StringBuilder stringBuilder = new StringBuilder();
        //    for (int i = 0; i < bytes.Length; i += 2)
        //    {
        //        // 取两个字符，每个字符都是右对齐。
        //        stringBuilder.AppendFormat("u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
        //    }
        //    return stringBuilder.ToString();
        //}

        ///// <summary>
        ///// Unicode转字符串
        ///// </summary>
        ///// <returns>The to string.</returns>
        ///// <param name="unicode">Unicode.</param>
        //public static string UnicodeToString(string unicode)
        //{
        //    string resultStr = "";
        //    string[] strList = unicode.Split('u');
        //    for (int i = 1; i < strList.Length; i++)
        //    {
        //        resultStr += (char)int.Parse(strList[i], System.Globalization.NumberStyles.HexNumber);
        //    }
        //    return resultStr;
        //}
    }
}
