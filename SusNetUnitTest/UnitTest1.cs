using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SusNet2.Common.Utils;
using System.Text;
using Sus.Net.Common.Entity;
using System.Collections.Concurrent;

namespace SusNetUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var data = string.Format("933304-9BUY,010,28,任务1867件,单位1件,累计出10000件,今日出213件");

            var hexData = SusNet2.Common.Utils.HexHelper.ToHex(data, "utf-8", false);
            Console.WriteLine("16进制字符串----->" + hexData);
            Console.WriteLine("中文----->" + HexHelper.UnHex(hexData, "utf-8"));

            var hexBytes = HexHelper.strToToHexByte(hexData);
            var length = hexBytes.Length;
            Console.WriteLine("字节长度--->" + length);
            Console.WriteLine("中文----->" + HexHelper.UnHex(HexHelper.byteToHexStr(new byte[] { hexBytes[length - 2], hexBytes[length - 1] }), "utf-8"));

        }
        [TestMethod]
        public void TestMethod2()
        {
            var data = string.Format("1.92085,100,S");
            var hexData = SusNet2.Common.Utils.HexHelper.ToHex(data, "utf-8", false);
            Console.WriteLine("16进制字符串----->" + hexData);
            Console.WriteLine("中文----->" + HexHelper.UnHex(hexData, "utf-8"));

            var hexBytes = HexHelper.strToToHexByte(hexData);
            var length = hexBytes.Length;
            Console.WriteLine("字节长度--->" + length);
            Console.WriteLine("中文----->" + HexHelper.UnHex(HexHelper.byteToHexStr(new byte[] { hexBytes[length - 2], hexBytes[length - 1] }), "utf-8"));
        }
        [TestMethod]
        public void TestHex()
        {
            //一个字节最大存10进制的数为255
            var data = 256;//65535;
            var hex = HexHelper.tenToHexString(data);
            Console.WriteLine(hex);
            var len = HexHelper.strToToHexByte(hex).Length;
            Console.WriteLine(len);
        }
        [TestMethod]
        public void TestFormat()
        {
            var data = string.Format("{0:x0000000000}", "3BDF25BC").PadLeft(10, '0');
            //var data = string.Format("{0:0000000000}", 1935110);
            Console.WriteLine(data);
            //var data2 = string.Format("{0:0001935110}", 6);

            //Console.WriteLine(data2);
            //var dd = 10 / 3;
            //Console.WriteLine("余数---->"+dd);
        }
        [TestMethod]
        public void TestHexConvert()
        {
            var data = 1;
            var data2 = 255;
            Console.WriteLine("data--->" + HexHelper.tenToHexString(data));
            Console.WriteLine("data2--->" + HexHelper.tenToHexString(data2));
            var dexBytes = System.BitConverter.GetBytes(data2);

            Console.WriteLine("整型255的字节长度为:" + dexBytes.Length);
            Console.WriteLine("16进制的255字节长度为:" + HexHelper.strToToHexByte("FF").Length);
        }
        [TestMethod]
        public void Test2()
        {
            byte[] bytes = { 0x20, 0x00, 0x01, 0xD8, 0x68, 0x00, 0xA7, 0x00 };
            Encoding enc = new UnicodeEncoding(false, true, true);

            try
            {
                string value = enc.GetString(bytes);
                Console.WriteLine();
                Console.WriteLine("'{0}'", value);
            }
            catch (DecoderFallbackException e)
            {
                Console.WriteLine("Unable to decode {0} at index {1}",
                                  ShowBytes(e.BytesUnknown), e.Index);
            }
        }
        private static string ShowBytes(byte[] bytes)
        {
            string returnString = null;
            foreach (var byteValue in bytes)
                returnString += String.Format("0x{0:X2} ", byteValue);

            return returnString.Trim();
        }

        [TestMethod]
        public void TestUnicode()
        {
            var data = "933304-9BUY,010,28,任务1863件,单位1件,累计出1117件,今日出213件";
            //var rslt=UnicodeUtils.StringToUnicode(data);
            //Console.WriteLine(rslt);

            //var bData = System.Text.UnicodeEncoding.Unicode.GetBytes("件");

            ////计算字节数组对应的字符数组长度;
            //var decodeer = System.Text.UnicodeEncoding.Unicode.GetDecoder();
            //int charSize = System.Text.UnicodeEncoding.Unicode.GetDecoder().GetCharCount(bData, 0, bData.Length);
            //Char[] chs = new char[charSize];
            ////进行字符转换;
            //int charLength = decodeer.GetChars(bData, 0, bData.Length, chs, 0);
            //Console.WriteLine(new String(chs));

            //var hexSendData = HexHelper.ToHex(data);
            var bData = UnicodeUtils.GetHexFromChs(data);
            Console.WriteLine(bData);
            Console.WriteLine(UnicodeUtils.GetChsFromHex(bData));

            // var reData = UnicodeUtils.StringByDecoder(bData);//bData);
            // Console.WriteLine("解码数据------>"+reData);

            //var utf8Data= System.Text.UnicodeEncoding.GetEncoding("utf-8").GetBytes(data);
            // Console.WriteLine("Unicode->utf-8:"+ utf8Data.Length);

            // var hangerNo = "1004479932";//1840236;

            // var hexHangerNo=HexHelper.tenToSixteen(hangerNo);
            // Console.WriteLine("-------------->"+hexHangerNo);
            // Console.WriteLine("--------------->Len:"+HexHelper.strToToHexByte(hexHangerNo).Length);
            // var uLen = HexHelper.tenToHexString(hangerNo); //UnicodeUtils.GetBytesByUnicode(hangerNo.ToString());
            // Console.WriteLine("--->10-->len:"+uLen.Length);
        }
        [TestMethod]
        public void TestData2()
        {
            var begin = HexHelper.HexToTen("0100");
            var end = HexHelper.HexToTen("0120");
            Console.WriteLine(begin);
            Console.WriteLine(end);
            Console.WriteLine(HexHelper.tenToHexString(1));
        }
        ConcurrentDictionary<string, ClientUserInfo> clients = new ConcurrentDictionary<string, ClientUserInfo>();
        [TestMethod]
        public void TestConcurrentDictionary()
        {
            //for (var index = 1; index < 10; index++)
            //{
            //    clients.ad
            //}
        }
    }
}
