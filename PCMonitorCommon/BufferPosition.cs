using System;
using System.Collections.Generic;
using System.Text;

namespace PCMonitorCommon;
public struct BufferPosition
{
    public int offset;
    public int position;

    public static BufferPosition operator ++(BufferPosition b)
    {
        b.position++;
        return b;
    }

    public static BufferPosition operator --(BufferPosition b)
    {
        b.position--;
        return b;
    }

    public static BufferPosition operator +(BufferPosition b, int i)
    {
        b.position += i;
        return b;
    }

    public static BufferPosition operator -(BufferPosition b, int i)
    {
        b.position -= i;
        return b;
    }

    public static implicit operator int(BufferPosition buf)
    {
        return buf.position + buf.offset;
    }
}
