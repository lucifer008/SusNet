using System;
using System.Collections.Generic;
using System.Text;

namespace Sus.Net.Common.Message
{
    public interface IMessage
    {
        byte[] Encode();
        string Describe();
    }
}
