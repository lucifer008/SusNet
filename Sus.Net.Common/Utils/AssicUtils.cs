using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sus.Net.Common.Utils
{
   public class AssicUtils
    {

        /// <summary>
        /// ASSIC码解码
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static string DecodeByBytes(byte[] buf) {
            return System.Text.Encoding.ASCII.GetString(buf);
        }

        /// <summary>
        /// ASSIC码编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] EncodeByStr(string content) {
            return Encoding.Default.GetBytes(content);
        }
    }
}
