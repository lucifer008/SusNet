using SusNet.Services.Da.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet.Services.Service
{
    public interface IBridgeService
    {
        bool IsBridgeStating(int mainTrackNumber,int statingNo, ref BridgeSet bridgeSe);
    }
}
