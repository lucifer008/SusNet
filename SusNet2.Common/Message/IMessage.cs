using System;
using System.Collections.Generic;
using System.Text;

namespace SusNet2.Common.Message
{
    public interface IMessage
    {
        byte[] Encode();
        string Describe();
    }
}
