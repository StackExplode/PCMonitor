using System;
using System.Collections.Generic;
using System.Text;

namespace PCMonitorCommon.Entity;



public enum FuncType : byte
{
    Unkown = 0x00,
    HeartBeat = 0x01,
    HeartBeatRes = 0x81,

#if DEBUG
    Test1 = 0x90,
    Test2 = 0x91,
#endif
    Cheat = 0x7F,
    CheatResponse = 0xFF
}
