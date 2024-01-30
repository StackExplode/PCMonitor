using System;
using System.Collections.Generic;
using System.Text;

namespace PCMonitorCommon;
public abstract class BufferManager
{
    protected byte[] buffer;
    protected BufferPosition ptr;
    public int Position => ptr.position;
    public bool IsEndianReversed { get; set; } = false;
    public static bool IsMachineLittleEndian => BitConverter.IsLittleEndian;

    public void Seek(int offset) => ptr += offset;
    public void SeekTo(int position) => ptr.position = position;
    public void SeekEnd() => ptr.position = buffer.Length - 1;
    public void SeekStart() => ptr.position = 0;


    public void ReverseBuffer() => buffer.AsSpan()[ptr.offset..].Reverse();
    public void ReverseBuffer(int start) => buffer.AsSpan()[(int)(ptr + start)..].Reverse();
    public void ReverseBuffer(int start, int len) => buffer.AsSpan((int)(ptr + start), len).Reverse();

}
