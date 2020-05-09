using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet.Services.Utils
{
    public class GUIDHelper
    {
        public static string GetGuidString() {
            var guid = Guid.NewGuid().ToString("N");
            return guid;
        }
    }
}
