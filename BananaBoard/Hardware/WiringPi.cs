using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BananaBoard.Hardware;
internal static partial class WiringPi
{
    [DllImport("libwiringPi.so")]
    public static extern int wiringPiSetup();

    [DllImport("libwiringPi.so")]
    public static extern void pinMode(int pin, int mode);
    [DllImport("libwiringPi.so")]
    public static extern void pullUpDnControl(int pin, int pud);
    [DllImport("libwiringPi.so")]
    public static extern int digitalRead(int pin);
    [DllImport("libwiringPi.so")]
    public static extern void digitalWrite(int pin, int value);

    [DllImport("libc")]
    public static extern void close(int fd);

}
