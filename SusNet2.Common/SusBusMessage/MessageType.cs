using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.SusBusMessage
{
    /// <summary>
    /// 硬件消息分类
    /// </summary>
   public sealed class MessageType
    {
        public static readonly MessageType StatMainTrack = new MessageType("10", "启动主轨道");
        public static readonly MessageType Size = new MessageType("1", "尺码");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_value">十六进制字符串</param>
        /// <param name="desption"></param>
        private MessageType(string _value, string desption)
        {
            Value = _value;
            Desption = desption;
        }
        public string Value { set; get; }
        public string Desption { set; get; }
    }
}
