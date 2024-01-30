using PCMonitorCommon.Test;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PCMonitorCommon.Entity;

public interface IBinaryComData
{

    //protected void ReadFromData(BufferReader reader);
    //protected void WriteToData(BufferWriter writer);
    public byte[] ToArray(bool refresh = false);
    public void FromArray(byte[] data, bool copy = true);
}

public abstract class ComData : IBinaryComData
{
    public const ushort FIXED_LENGTH = 8;
    public virtual byte Version => 0x01;
    public byte OriginID { get; set; }
    public byte DestinationID { get; set; }
    public ushort DataLen => (ushort)(CarrierLen + FIXED_LENGTH);
    public abstract FuncType FuncCode { get; }
    public ushort CRCCode { get; set; }

    public bool IsInited => buffer is not null;

    protected abstract int CarrierLen { get; }
    protected byte[] buffer;

    public ReadOnlySpan<byte> RawBuffer => buffer.AsSpan();
    public ReadOnlySpan<byte> RawCarrier => buffer.AsSpan(6, CarrierLen);

    protected abstract void ReadFromData(BufferReader reader);

    protected abstract void WriteToData(BufferWriter writer);

    public unsafe ushort CalculateCRC16()
    {
        ushort rt = 0;
        fixed (byte* ptr = this.buffer)
        {
            rt = Obsolete.ComData.Get_CRC16(ptr, (ushort)(this.buffer.Length - 2));
        }
        return rt;
    }

    public bool CheckCRC16()
    {
        return this.CRCCode == CalculateCRC16();
    }

    protected void FillCRCCode()
    {
        ref var crc = ref Unsafe.As<byte, ushort>(ref buffer.AsSpan()[^2]);
        crc = CalculateCRC16();
    }

    public byte[] ToArray(bool refresh = false)
    {
        if(!IsInited || refresh)
        {
            buffer = new byte[DataLen];
            BufferWriter bw = new BufferWriter(buffer);
            bw.Write(Version);
            bw.Write(OriginID);
            bw.Write(DestinationID);
            bw.Write(DataLen);
            bw.Write((byte)FuncCode);
            WriteToData(bw);
            FillCRCCode();
        }

        return buffer;
    }

    public void FromArray(byte[] data, bool copy = true)
    {
        if(copy)
            buffer = new byte[(int)data.Length];
        else
            buffer = data;

        BufferReader br = new BufferReader(buffer);
        byte ver = br.Read<byte>();
        if (ver != Version)
            throw new Exception($"Data version({ver}) not corresponded to local version({Version})!");
        OriginID = br.Read<byte>();
        DestinationID = br.Read<byte>();
        ushort len = br.Read<ushort>();
        if (data.Length != len)
            throw new Exception($"Data length field value({len}) not equal to real buffer length({data.Length})!");
        FuncType fun = (FuncType)br.Read<byte>();
        if(fun != FuncCode)
            throw new Exception($"Data function field({fun}) not corresponded to this class({FuncCode})!");
        ReadFromData(br);
        CRCCode = br.Read<ushort>();
    }

    public virtual void Init(params object[] args)
    {

    }
}

