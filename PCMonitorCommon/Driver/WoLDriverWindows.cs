using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace PCMonitorCommon.Driver;
public class WoLDriverWindows : IDisposable
{
    private Socket rawsocket;
    private NetworkInterface inter;
    private bool disposedValue;

    private WoLDriverWindows()
    {
        bool sys = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        if (!sys)
            throw new Exception("This class only support Windows platform!");
    }

    public WoLDriverWindows(string mymac):this()
    {
        rawsocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Raw);
        rawsocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
        inter = GetInterface(mymac);
        rawsocket.Bind(new System.Net.IPEndPoint(inter.GetIPProperties().UnicastAddresses[0].Address, 0));
    }

    public WoLDriverWindows(int index):this()
    {
        rawsocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Raw);
        rawsocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
        inter = GetInterface(index);
        rawsocket.Bind(new System.Net.IPEndPoint(IPAddress.Broadcast , 0));
    }


    private NetworkInterface GetInterface(string paddr)
    {
        paddr = paddr.ToUpper()
                    .Replace(" ", "")
                    .Replace("-", "")
                    .Replace(":", "")
                    .Trim();
        NetworkInterface[] networkInterface = NetworkInterface.GetAllNetworkInterfaces();
        foreach(var inter in  networkInterface)
        {
            if(inter.GetPhysicalAddress() == PhysicalAddress.Parse(paddr))
                return inter;
        }
        return null;
    }

    private NetworkInterface GetInterface(int id)
    {
        NetworkInterface[] networkInterface = NetworkInterface.GetAllNetworkInterfaces();

        foreach (var inter in networkInterface)
        {
            IPInterfaceProperties adapterProperties = inter.GetIPProperties();
            IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();
            if ((p is not null) && (p.Index == id))
                return inter;
        }
        return null;
    }

    public void SendWoL(string paddr)
    {
        paddr = paddr.ToUpper()
                    .Replace(" ", "")
                    .Replace("-", "")
                    .Replace(":", "")
                    .Trim();
        PhysicalAddress pa = PhysicalAddress.Parse(paddr);
        byte[] wol = pa.GetAddressBytes();
        byte[] dest = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        byte[] org = inter.GetPhysicalAddress().GetAddressBytes();
        byte[] type = { 0x08, 0x42 };   //WoL

        byte[] password = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

        byte[] data = new byte[116];

        int offset = 0;
        Buffer.BlockCopy(dest, 0, data, offset, 6); offset += 6;
        Buffer.BlockCopy(org, 0, data, offset, 6); offset += 6;
        Buffer.BlockCopy(type, 0, data, offset, 2); offset += 2;
        Buffer.BlockCopy(password, 0, data, offset, 6); offset += 6;
        for (int i=0; i<16; i++)
        {
            Buffer.BlockCopy(wol, 0, data, offset, 6); offset += 6;
        }

        rawsocket.SendTo(data, data.Length, SocketFlags.None, new IPEndPoint(IPAddress.Broadcast, 0) );
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                rawsocket.Close();
                // TODO: 释放托管状态(托管对象)
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~WoLDriver()
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
