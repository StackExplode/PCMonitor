using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PCMonitorCommon.Entity.Obsolete;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct UnknownData : IComDataField
{

    public FuncType FuncCode => FuncType.Unkown;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct Test1Data : IComDataField
{
    public byte Data1;
    public byte Data2;
    public FuncType FuncCode => FuncType.Test1;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct HeartBeatData : IComDataField
{
    public byte ChildrenLen;
    public fixed byte Children[10];
    public FuncType FuncCode => FuncType.HeartBeat;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct HeartBeatResData : IComDataField
{
    public byte ErrorCode;
    public FuncType FuncCode => FuncType.HeartBeatRes;
}
