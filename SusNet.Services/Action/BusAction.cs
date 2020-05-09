using SusNet.Services.Da.Domain;
using SusNet.Services.Service.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet.Services.Action
{
    public class BusAction
    {
        public static readonly BusAction Instance = new BusAction();
        public bool IsBridgeStating(int mainTrackNumber, int statingNo,ref BridgeSet bridgeSet) {
            return new BridgeServiceImpl().IsBridgeStating(mainTrackNumber,statingNo,ref bridgeSet);
        }
    }
}
