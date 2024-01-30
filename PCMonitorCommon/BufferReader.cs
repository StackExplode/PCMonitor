using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace PCMonitorCommon;
public class BufferReader : BufferManager
{
    public BufferReader(byte[] buffer, int offset)
    {
        this.buffer = buffer;
        this.ptr.offset = offset;
        this.ptr.position = 0;
    }

    public BufferReader(byte[] buffer) : this(buffer, 0) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Read<T>() where T : struct
    {
        int len = Unsafe.SizeOf<T>();
        T rt = Unsafe.As<byte, T>(ref buffer[ptr]);

        if (IsEndianReversed)
        {
            T[] fuck = new T[] { rt };
            Span<byte> fuck2 = MemoryMarshal.AsBytes<T>(fuck.AsSpan(0,1));
            fuck2.Reverse();
            ptr += len;
            return fuck[0];
        }
        else
        {
            ptr += len;
            return rt;
        }

        
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T ReadAsRef<T>(bool autoreverse = false) where T : struct
    {
        int len = Unsafe.SizeOf<T>();

        if (IsEndianReversed && autoreverse)
        {
            Span<byte> fuck2 = buffer.AsSpan(ptr, len);
            fuck2.Reverse();
        }

        ref var rt = ref Unsafe.As<byte, T>(ref buffer[ptr]);
        ptr += len;
        return ref rt;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T[] ReadArray<T>(int length) where T : struct
    {
        int len = Unsafe.SizeOf<T>();
        int cnt = length;
        T[] values = new T[cnt];

        if (IsEndianReversed)
        {
            for (int i = 0; i < cnt; i++)
            {
                var span = MemoryMarshal.AsBytes(values.AsSpan(i, 1));
                var span2 = buffer.AsSpan(ptr + i * len, len);
                for (int j = 0; j < len; j++)
                {
                    span[ptr++] = span2[^j];
                }
            }
        }
        else
        {
            Unsafe.CopyBlock(Unsafe.AsPointer(ref values[0]), Unsafe.AsPointer(ref buffer[ptr]), (uint)(len * cnt));
            ptr += len * cnt;
        }
            
        return values;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Span<T> ReadAsSpan<T>(int length, bool autoreverse = false) where T : struct
    {
        int len = Unsafe.SizeOf<T>();
        int cnt = length;

        if (IsEndianReversed && autoreverse)
        {
            for (int i = 0; i < cnt; i++)
            {
                var span = buffer.AsSpan(ptr + i * len, len);
                span.Reverse();
            }
        }

        var rt = MemoryMarshal.Cast<byte, T>(buffer.AsSpan(ptr, cnt * len));
        ptr += len * cnt;

        return rt;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ReadString(int length, Encoding encode = null)
    {
        if (encode is null)
            encode = Encoding.Default;
        string rt = encode.GetString(buffer, ptr, length);
        ptr += length;
        return rt;
    }

    public void ReadCollection<T>(ICollection<T> list,int length) where T : struct
    {
        int len = Unsafe.SizeOf<T>();
        int cnt = length;
        for (int i = 0; i < cnt; i++)
        {
            list.Add(this.ReadAsRef<T>());
        }
    }

    public void ReadDictionary<K,V>(IDictionary<K,V> dic, int length) where K : struct where V : struct
    {
        int len1 = Unsafe.SizeOf<K>();
        int len2 = Unsafe.SizeOf<V>();
        
        int cnt = length;
        for (int i = 0; i < cnt; i++)
        {
            ref K key = ref this.ReadAsRef<K>();
            ref V val = ref this.ReadAsRef<V>();
            dic.Add(key, val);
        }
    }



    public IEnumerable<T> Iterate<T>(int length) where T : struct
    {
        int len = Unsafe.SizeOf<T>();
        int cnt = length;
        for (int i = 0; i < cnt; i++)
        {
            yield return this.ReadAsRef<T>();  
        }
    }



    public IEnumerable<KeyValuePair<K, V>> IterateKeyValuePairs<K,V>(int length) where K : struct where V : struct
    {
        int len1 = Unsafe.SizeOf<K>();
        int len2 = Unsafe.SizeOf<V>();
        int cnt = length;
        for (int i = 0; i < cnt; i++)
        {
            K key = this.ReadAsRef<K>();
            V val = this.ReadAsRef<V>();
            yield return new KeyValuePair<K, V>(key, val);
        }
    }

    public KeyValuePair<K, V> ReadKeyValuePair<K, V>() where K : struct where V : struct
    {
        int len1 = Unsafe.SizeOf<K>();
        int len2 = Unsafe.SizeOf<V>();
        ref K key = ref this.ReadAsRef<K>();
        ref V val = ref this.ReadAsRef<V>();
        return new KeyValuePair<K, V>(key, val);
    }

}
