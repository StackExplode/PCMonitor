using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Net.NetworkInformation;


namespace PCMonitorCommon.Driver;
public class WoLDriverLinux : IDisposable
{
    private WoLDriverLinux()
    {
        bool sys = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        if (!sys)
            throw new Exception("This class only support Linux platform!");
    }

    Socket _socket;
    int nic_index;
    private bool disposedValue;

    public WoLDriverLinux(int nic_index) : this()
    {
        this.nic_index = nic_index;
        _socket = new Socket((AddressFamily)65536, SocketType.Raw, ProtocolType.Raw);
        var address = new LLEndPoint(nic_index);
        _socket.Bind(address);
    }

    private PhysicalAddress GetMacAddrByIndex(int index)
    {
        NetworkInterface[] networkInterface = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var nic in networkInterface)
        {
            IPInterfaceProperties adapterProperties = nic.GetIPProperties();
            IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();
            if (p is not null)
            {
                if(p.Index == index)
                    return nic.GetPhysicalAddress();
            }
        }
        return null;
    }

    public void SendWoL(string paddr, byte[] pass = null)
    {
        paddr = paddr.ToUpper()
                .Replace(" ", "")
                .Replace("-", "")
                .Replace(":", "")
                .Trim();
        PhysicalAddress pa = PhysicalAddress.Parse(paddr);
        byte[] wol = pa.GetAddressBytes();
        byte[] dest = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        byte[] org = GetMacAddrByIndex(nic_index).GetAddressBytes();
        byte[] type = { 0x08, 0x42 };   //WoL

        byte[] password;
        if (pass != null)
        {
            password = pass;
        }
        else
        {
            password = new byte[]{ 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        }


        byte[] data = new byte[116];

        int offset = 0;
        Buffer.BlockCopy(dest, 0, data, offset, 6); offset += 6;
        Buffer.BlockCopy(org, 0, data, offset, 6); offset += 6;
        Buffer.BlockCopy(type, 0, data, offset, 2); offset += 2;
        Buffer.BlockCopy(password, 0, data, offset, 6); offset += 6;
        for (int i = 0; i < 16; i++)
        {
            Buffer.BlockCopy(wol, 0, data, offset, 6); offset += 6;
        }

        _socket.Send(data);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _socket.Close();
                _socket.Dispose();
                // TODO: 释放托管状态(托管对象)
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~WoLDriverLinux()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

internal class LLEndPoint : EndPoint
{
    private Int32 _ifIndex;

    public LLEndPoint(int interfaceIndex)
    {
        _ifIndex = interfaceIndex;
    }

    public override SocketAddress Serialize()
    {
        var a = new SocketAddress((AddressFamily)65536, 20);
        byte[] asBytes = BitConverter.GetBytes(_ifIndex);
        a[4] = asBytes[0];
        a[5] = asBytes[1];
        a[6] = asBytes[2];
        a[7] = asBytes[3];

        return a;
    }
}
