using PCMonitorCommon.Entity;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace PCMonitorCommon.Test;



interface IBinaryComData
{
    //protected byte[] buffer;
    //public abstract byte[] ToArray(bool createnew = false);
    //public abstract void FromArray(byte[] data,bool copy = false);

    public void ReadFromData(dynamic fake);
    public void WriteToData(BufferWriter writer);
    public byte[] ToArray(bool refresh = false);
    public void FromArray(byte[] data);
}

abstract class ComData : IBinaryComData
{
    private byte[] buffer;
    public byte Header1 { get; set; } 
    public byte Header2 { get; set; }

    internal byte[] RawBuffer => buffer;

    public abstract int DataLength { get; }
    public abstract void ReadFromData(dynamic fake);
    public abstract void WriteToData(BufferWriter writer);

    public byte[] ToArray(bool refresh = false)
    {
        if (!refresh && buffer is not null)
            return buffer;

        buffer = new byte[DataLength + 2];
        BufferWriter writer = new BufferWriter(buffer);
        WriteToData(writer);

        return buffer;
    }

    public void FromArray(byte[] data)
    {
        throw new NotImplementedException();
    }
}

class TestComData : ComData
{
    public byte TestData1 { get; set; }
    public byte TestData2 { get; set; }

    public override int DataLength => throw new NotImplementedException();

    public override void ReadFromData(dynamic fake)
    {
        throw new NotImplementedException();
    }

    public override void WriteToData(BufferWriter writer)
    {
        throw new NotImplementedException();
    }
}


class BufferWriter
{
    byte[] buffer;
    int ptr = 0;
    public BufferWriter(byte[] buf) { buffer = buf; }

    public void WriteByte(byte b)
    {
        buffer[ptr++] = b;
        
    }


}



public static class TestTasks
{


    public static unsafe void Run()
    {

        var a = "dawd"u8;


    }
}
