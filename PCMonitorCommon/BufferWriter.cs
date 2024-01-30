using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace PCMonitorCommon;


public class BufferWriter : BufferManager
{
    

    public BufferWriter(byte[] buffer, int offset)
    {
        this.buffer = buffer;
        this.ptr.offset = offset;
        this.ptr.position = 0;
    }

    public BufferWriter(byte[] buffer):this(buffer, 0) { }




    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteRef<T>(ref T value) where T : struct
    {
        int len = Unsafe.SizeOf<T>();
        var span = buffer.AsSpan(ptr, len);
        MemoryMarshal.Write(span, ref value);
        ptr += len;
        if (IsEndianReversed)
            span.Reverse();

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write<T>(T value) where T : struct
    {
        int len = Unsafe.SizeOf<T>();
        var span = buffer.AsSpan(ptr, len);
        MemoryMarshal.Write(span, ref value);
        ptr += len;
        if (IsEndianReversed)
            span.Reverse();
        
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Write<T>(T[] values) where T : struct 
    {
        int len = Unsafe.SizeOf<T>();
        int cnt = values.Length;
        //var span = buffer.AsSpan(ptr, len);
        //Buffer.BlockCopy(values,0,buffer,ptr,len);
        if (IsEndianReversed)
        {
            for(int i = 0; i < cnt; i++)
            {
                var span = MemoryMarshal.AsBytes(values.AsSpan(i, 1));
                for(int j= 0; j < len; j++)
                {
                    buffer[ptr++] = span[^j];
                }
            }
        }
        else
        {
            Unsafe.CopyBlock(Unsafe.AsPointer(ref buffer[ptr]), Unsafe.AsPointer(ref values[0]), (uint)(len * cnt));
            ptr += len * cnt;
        }

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Write<T>(Span<T> span) where T : struct
    {
        int len = Unsafe.SizeOf<T>();
        int cnt = span.Length;

        if (IsEndianReversed)
        {
            for (int i = 0; i < cnt; i++)
            {
                var span2 = MemoryMarshal.AsBytes(span.Slice(i, 1));
                for (int j = 0; j < len; j++)
                {
                    buffer[ptr++] = span2[^j];
                }
            }
        }
        else
        {
            Unsafe.CopyBlock(Unsafe.AsPointer(ref buffer[ptr]), Unsafe.AsPointer(ref span[0]), (uint)(len * cnt));
            ptr += len * cnt;
        }
            
    }


    public void Write<T>(IList<T> values) where T : struct
    {
        int cnt = values.Count;
        for (int i = 0; i < cnt; i++)
        {
            var ele = values[i];
            WriteRef<T>(ref ele);
        }
    }

    public void Write<T>(IEnumerable<T> values, bool order = false) where T : struct
    {
        int len = Unsafe.SizeOf<T>();
        
        if(order)
        {
            int cnt = values.Count();
            for (int i=0; i<cnt; i++)
            {
                var ele = values.ElementAt(i);
                WriteRef<T>(ref ele);
            }
        }
        else
        {
            foreach (var value in values)
            {
                var ele = value;
                WriteRef<T>(ref ele);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(string s, Encoding encode = null)
    {
        if (encode is null)
            encode = Encoding.Default;
        byte[] arr = encode.GetBytes(s);
        Write<byte>(arr);
    }
    public void Write<K, V>(IEnumerable<KeyValuePair<K, V>> dic, bool order = false) where K : struct where V : struct
    {
        int len1 = Unsafe.SizeOf<K>();
        int len2 = Unsafe.SizeOf<V>();
        
        if (order)
        {
            int cnt = dic.Count();
            for (int i = 0; i < cnt; i++)
            {
                var ele = dic.ElementAt(i);
                var key = ele.Key;
                var value = ele.Value;
                WriteRef<K>(ref key);
                WriteRef<V>(ref value);
            }
        }
        else
        {
            foreach (var ele in dic)
            {
                var key = ele.Key;
                var value = ele.Value;
                WriteRef<K>(ref key);
                WriteRef<V>(ref value);
            }
        }
    }
#if !NET
    [Obsolete("Reflection is slow!")]
#endif
    public void Write<T>(List<T> list) where T : struct
    {
#if NET
        Span<T> span = CollectionsMarshal.AsSpan(list);
        this.Write(span);
#else
        FieldInfo fi = list.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
        T[] items = (T[])fi.GetValue(list);
        Write(items);
#endif
    }

#if HIGH_LEVEL
    public void Write<K>(IEnumerable<KeyValuePair<K, string>> dic, Encoding encode = null, bool order = false) where K : struct
    {
        int len1 = Unsafe.SizeOf<K>();

        if (order)
        {
            int cnt = dic.Count();
            for (int i = 0; i < cnt; i++)
            {
                var ele = dic.ElementAt(i);
                var key = ele.Key;
                var value = ele.Value;
                WriteRef<K>(ref key);
                Write(value, encode);
            }
        }
        else
        {
            foreach (var ele in dic)
            {
                var key = ele.Key;
                var value = ele.Value;
                WriteRef<K>(ref key);
                Write(value, encode);
            }
        }
    }

    public void Write<K>(IEnumerable<KeyValuePair<string, K>> dic, Encoding encode = null, bool order = false) where K : struct
    {
        int len1 = Unsafe.SizeOf<K>();

        if (order)
        {
            int cnt = dic.Count();
            for (int i = 0; i < cnt; i++)
            {
                var ele = dic.ElementAt(i);
                var key = ele.Key;
                var value = ele.Value;
                Write(key, encode);
                WriteRef<K>(ref value);
                
            }
        }
        else
        {
            foreach (var ele in dic)
            {
                var key = ele.Key;
                var value = ele.Value;
                Write(key, encode);
                WriteRef<K>(ref value);
            }
        }
    }

    public void Write(IEnumerable<KeyValuePair<string, string>> dic, Encoding encode, bool order = false)
    {

        if (order)
        {
            int cnt = dic.Count();
            for (int i = 0; i < cnt; i++)
            {
                var ele = dic.ElementAt(i);
                var key = ele.Key;
                var value = ele.Value;
                Write(key, encode);
                Write(value, encode);

            }
        }
        else
        {
            foreach (var ele in dic)
            {
                var key = ele.Key;
                var value = ele.Value;
                Write(key, encode);
                Write(value, encode);
            }
        }
    }
#endif

}
