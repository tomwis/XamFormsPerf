using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamFormsPerf
{
    public static class DeviceInfo
    {
        public static string Get()
        {
            var di = Plugin.DeviceInfo.CrossDeviceInfo.Current;
            return $"{di.Model};{di.Version}";
        }
    }
}
