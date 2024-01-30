using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BananaBoard.Hardware;
internal static class WiringPiSPI
{
    [DllImport("libwiringPi.so")]
    public static extern int wiringPiSPIGetFd(int channel);

    [DllImport("libwiringPi.so")]
    public static extern int wiringPiSPIDataRW(int channel,[In][Out] byte[] data, int len);

    [DllImport("libwiringPi.so")]
    public static extern int wiringPiSPISetupMode(int channel, int speed, int mode);

    [DllImport("libwiringPi.so")]
    public static extern int wiringPiSPISetup(int channel, int speed);

}
