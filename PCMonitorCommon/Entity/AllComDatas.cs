using System;
using System.Collections.Generic;
using System.Text;

namespace PCMonitorCommon.Entity;
public class HeartbeatData : ComData
{
    public override FuncType FuncCode =>  FuncType.HeartBeat;

    protected override int CarrierLen => ChildrenLen + 1;

    public byte ChildrenLen => (byte)Children.Count;

    public List<byte> Children { get; private set; } = new List<byte>();

    protected override void ReadFromData(BufferReader reader)
    {
        byte len = reader.Read<byte>();
        Children.Clear();
        reader.ReadCollection<byte>(Children, len);
    }

    protected override void WriteToData(BufferWriter writer)
    {
        writer.Write(ChildrenLen);
        writer.Write((IList<byte>)Children);
    }
}

public class CheatData : ComData
{
    public override FuncType FuncCode => FuncType.Cheat;

    protected override int CarrierLen => 1;

    public byte CData { get; set; }

    protected override void ReadFromData(BufferReader reader)
    {
        CData = reader.Read<byte>();
    }

    protected override void WriteToData(BufferWriter writer)
    {
        writer.Write<byte>(CData);
    }
}

public class CheaResponsetData : CheatData
{
    public override FuncType FuncCode => FuncType.CheatResponse;

}
