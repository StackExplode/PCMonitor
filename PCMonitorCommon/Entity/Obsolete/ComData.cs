using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using PCMonitorCommon.Entity.Obsolete;

namespace PCMonitorCommon.Entity.Obsolete;

public enum FuncType : byte
{
    Unkown = 0x00,
    HeartBeat = 0x01,
    HeartBeatRes = 0x81,

#if DEBUG
    Test1 = 0x90,
    Test2 = 0x91
#endif
}

public interface IComDataField
{
    public FuncType FuncCode { get; }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ComDataModel<T> where T : IComDataField
{
    public byte Version = 0x01;
    public byte OriginID;
    public byte DestinationID;
    public ushort DataLen;
    public FuncType FuncCode;
    public T Data;
    public ushort CheckCode;

    public ComDataModel()
    {
    }
}



public abstract partial class ComData
{
    protected const int HEAD_TAIL_LEN = 8;
    protected byte[] buffer;

    public ReadOnlyCollection<byte> RawBuffer => Array.AsReadOnly(buffer);

    public static unsafe ComData<TData> Create<TData>(TData data) where TData : IComDataField
    {

        ComData<TData> rt = new ComData<TData>();
        ComDataModel<TData> dt = new ComDataModel<TData>();
        dt.Data = data;
        dt.DataLen = (ushort)sizeof(TData);
        dt.FuncCode = data.FuncCode;
        rt.buffer = new byte[sizeof(ComDataModel<TData>)];
        MemoryMarshal.Write(rt.buffer.AsSpan(), ref dt);

        //ref byte stru = ref Unsafe.As<ComDataModel<TData>, byte>(ref dt);
        //Unsafe.CopyBlock(ref rt.buffer[0], ref stru, (uint)sizeof(ComDataModel<TData>));
        
        //             int datalen = sizeof(TData);
        //             rt.buffer = new byte[HEAD_TAIL_LEN + datalen];
        //             ref byte data_buff = ref Unsafe.As<TData, byte>(ref data);
        //             Unsafe.CopyBlock(ref rt.buffer[6], ref data_buff, (uint)datalen);

        //             rt.RawData.FuncCode = data.FuncCode;
        //             rt.RawData.DataLen = (ushort)datalen;
        return rt;
        
    }

    public static ComData<UnknownData> Create(byte[] buffer)
    {
        ComData<UnknownData> rt = new ComData<UnknownData>();
        rt.buffer = new byte[(int)buffer.Length];
        Buffer.BlockCopy(buffer, 0, rt.buffer, 0, buffer.Length);
        return rt;
    }

    public static ComData<TData> Create<TData>(byte[] buffer) where TData : IComDataField
    {
        ComData<TData> rt = new ComData<TData>();
        rt.buffer = new byte[(int)buffer.Length];
        Buffer.BlockCopy(buffer, 0, rt.buffer, 0, buffer.Length);
        return rt;
    }

    public ComData<T> UnsafeAs<T>() where T : IComDataField
    {
        return Unsafe.As<ComData<T>>((ComData<UnknownData>)this);
    }

    protected ref ComDataModel<UnknownData> RawData
    {
        get
        {
            ref ComDataModel<UnknownData> data = ref Unsafe.As<byte, ComDataModel<UnknownData>>(ref buffer[0]);
            return ref data;
        }
    }

    public byte ProtocolVersion => RawData.Version;

    public byte OriginID
    {
        get => RawData.OriginID;
        set => RawData.OriginID = value;
    }

    public byte DestinationID
    {
        get => RawData.DestinationID;
        set => RawData.DestinationID = value;
    }

    public FuncType FuncCode => RawData.FuncCode;

    public ushort DataLength => RawData.DataLen;


    public ushort CRCCode => Unsafe.As<byte, ushort>(ref buffer.AsSpan()[^2]);

    

}

public class ComData<TData> : ComData where TData : IComDataField
{

    internal ComData() { }


    

    protected new ref ComDataModel<TData> RawData
    {
        get
        {
            ref ComDataModel<TData> data = ref Unsafe.As<byte, ComDataModel<TData>>(ref buffer[0]);
            return ref data;
        }
    }

    public ref TData Data
    {
        get
        {
            /*ref TData data = ref Unsafe.As<byte, TData>(ref RawData.PayLoad[0]);
            return ref data;*/
            return ref RawData.Data;
        }
    }

    public byte[] ToArray(bool createnew = false)
    {
        FillCRCCode();
        if (createnew)
            return buffer.ToArray();
        else
            return buffer;
    }

    public static explicit operator ComData<TData>(ComData<UnknownData> v)
    {
        return Unsafe.As<ComData<TData>>(v);
    }


}



