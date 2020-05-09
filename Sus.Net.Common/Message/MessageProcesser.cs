using System;
using System.Collections.Generic;
using System.Reflection;
using Sus.Net.Common.Utils;
using SuspeSys.Utils;

namespace Sus.Net.Common.Message
{
    public class MessageProcesser:SusLog
    {
        private MessageProcesser()
        {
        }

        public static MessageProcesser Instance { get { return Nested._instance; } }

        private readonly SortedList<string, List<byte>> receivedDataList = new SortedList<string, List<byte>>();
        private MessageProcesser RemoveCache(string key)
        {
            lock ( receivedDataList )
            {
                receivedDataList.Remove(key);
                return this;
            }
        }

        public List<Message> ProcessRecvData(string key,byte[] data)
        {
            try
            {
                lock ( receivedDataList )
                {
                    List<Message> messageList = new List<Message>();
                    List<byte> byteList;
                    if ( receivedDataList.ContainsKey(key) )
                    {
                        byteList = receivedDataList[key];
                    }
                    else
                    {
                        byteList = new List<byte>();
                        receivedDataList.Add(key,byteList);
                    }
                    byteList.AddRange(data);
                    //int offset;
                    try
                    {
                        messageList = Message.DecodeMessaage2(byteList);// Message.DecodeMessaage(byteList,out offset);
                        byteList.RemoveRange(0, byteList.Count);
                    }
                    catch ( Exception ex )
                    {
                        tcpLogInfo.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                        byteList.Clear();
                    }
                    return messageList;
                }
            }
            catch ( Exception ex )
            {
                tcpLogInfo.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                return new List<Message>();
            }
        }

        /// <summary>
        /// 单类懒加载
        /// </summary>
        private static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly MessageProcesser _instance = new MessageProcesser();
        }
    }
}
