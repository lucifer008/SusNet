using System;
using System.Collections.Generic;
using System.Text;

namespace SusNet2.Common.Message
{
    public interface IMessage1
    {
        byte[] Encode();
        string Describe();
    }
}
