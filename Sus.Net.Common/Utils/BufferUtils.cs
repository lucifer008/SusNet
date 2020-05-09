using System;
using System.Collections.Generic;
using System.Text;

namespace Sus.Net.Common.Utils
{
    public class BufferUtils
    {
        public byte[] Encode(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public string Decode(byte[] data,int offset,int count)
        {
            return Encoding.UTF8.GetString(data,offset,count);
        }

        public static short SwapInt16(short v)
        {
            return (short)( ( ( v & 0xff ) << 8 ) | ( ( v >> 8 ) & 0xff ) );
        }
        public static ushort SwapUInt16(ushort v)
        {
            return (ushort)( ( ( v & 0xff ) << 8 ) | ( ( v >> 8 ) & 0xff ) );
        }
        public static int SwapInt32(int v)
        {
            return (int)( ( ( SwapInt16((short)v) & 0xffff ) << 0x10 ) |
                          ( SwapInt16((short)( v >> 0x10 )) & 0xffff ) );
        }
        public static uint SwapUInt32(uint v)
        {
            return (uint)( ( ( SwapUInt16((ushort)v) & 0xffff ) << 0x10 ) |
                           ( SwapUInt16((ushort)( v >> 0x10 )) & 0xffff ) );
        }
        public static long SwapInt64(long v)
        {
            return (long)( ( ( SwapInt32((int)v) & 0xffffffffL ) << 0x20 ) |
                           ( SwapInt32((int)( v >> 0x20 )) & 0xffffffffL ) );
        }
        public static ulong SwapUInt64(ulong v)
        {
            return (ulong)( ( ( SwapUInt32((uint)v) & 0xffffffffL ) << 0x20 ) |
                            ( SwapUInt32((uint)( v >> 0x20 )) & 0xffffffffL ) );
        }

        public static bool GetBitValue(uint value,int index)
        {
            uint tag = 1;
            tag = tag << ( index );
            return ( value & tag ) > 0;
        }

        public static byte GetByteBitValue(params bool[] values)
        {
            byte result = 0;
            for ( int i = 0; i < values.Length; i++ )
                result = (byte)( result | ( ( values[i] ? 1 : 0 ) << i ) );
            return result;
        }

        public static ushort GetUShortBitValue(params bool[] values)
        {
            ushort result = 0;
            for ( int i = 0; i < values.Length; i++ )
                result = (ushort)( result | ( ( values[i] ? 1 : 0 ) << i ) );
            return result;
        }

        public static uint GetUIntBitValue(params bool[] values)
        {
            uint result = 0;
            for ( int i = 0; i < values.Length; i++ )
                result = (uint)( result | ( ( values[i] ? (uint)1 : (uint)0 ) << i ) );
            return result;
        }

        public static int GetCRC(byte[] array,int offset,int count)
        {
            if ( offset >= 0 && count <= array.Length )
            {
                int crc = 0;
                for ( int i = offset; i < count; i++ )
                {
                    crc ^= array[i];
                }
                return crc;
            }
            else
            {
                throw new ArgumentOutOfRangeException("start||end","数组越界");
            }
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] StrToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ","");
            if ( ( hexString.Length % 2 ) != 0 )
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for ( int i = 0; i < returnBytes.Length; i++ )
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2,2),16);
            return returnBytes;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if ( bytes != null )
            {
                for ( int i = 0; i < bytes.Length; i++ )
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
        public static string ToHex(string s,string charset,bool fenge)
        {
            if ( ( s.Length % 2 ) != 0 )
            {
                s += " ";//空格
                //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for ( int i = 0; i < bytes.Length; i++ )
            {
                str += string.Format("{0:X}",bytes[i]);
                if ( fenge && ( i != bytes.Length - 1 ) )
                {
                    str += string.Format("{0}",",");
                }
            }
            return str.ToLower();
        }

        ///<summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static string UnHex(string hex,string charset)
        {
            if ( hex == null )
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",","");
            hex = hex.Replace("\n","");
            hex = hex.Replace("\\","");
            hex = hex.Replace(" ","");
            if ( hex.Length % 2 != 0 )
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for ( int i = 0; i < bytes.Length; i++ )
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2,2),
                        System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!","hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }
    }
}
