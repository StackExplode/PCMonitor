using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BananaBoard.Hardware;
internal partial class WiringPi
{

    // wiringPi modes
    public const int WPI_MODE_PINS = 0;
    public const int WPI_MODE_GPIO = 1;
    public const int WPI_MODE_GPIO_SYS = 2;
    public const int WPI_MODE_PHYS = 3;
    public const int WPI_MODE_PIFACE = 4;
    public const int WPI_MODE_UNINITIALISED = -1;
    // Pin modes

    public const int INPUT = 0;
    public const int OUTPUT = 1;
    public const int PWM_OUTPUT = 2;
    public const int GPIO_CLOCK = 3;
    public const int SOFT_PWM_OUTPUT = 4;
    public const int SOFT_TONE_OUTPUT = 5;
    public const int PWM_TONE_OUTPUT = 6;

    public const int LOW = 0;
    public const int HIGH = 1;

    // Pull up/down/none

    public const int PUD_OFF = 0;
    public const int PUD_DOWN = 1;
    public const int PUD_UP = 2;

    // PWM

    public const int PWM_MODE_MS = 0;
    public const int PWM_MODE_BAL = 1;

    // Interrupt levels

    public const int INT_EDGE_SETUP = 0;
    public const int INT_EDGE_FALLING = 1;
    public const int INT_EDGE_RISING = 2;
    public const int INT_EDGE_BOTH = 3;

}
