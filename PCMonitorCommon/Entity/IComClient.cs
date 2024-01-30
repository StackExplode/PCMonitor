using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PCMonitorCommon.Entity;

public interface IComClient
{
    bool IsAlive { get; set; }
    void Disconnect();
}

public class PCMUDPClient : IComClient
{
    [Obsolete("Check alive of UDP Client is meaningless.")]
    public bool IsAlive { get; set; }

    public IPEndPoint Client => _udpclient;

    private IPEndPoint _udpclient;
    private readonly UdpClient server;

    public PCMUDPClient(IPEndPoint udpclient, UdpClient server)
    {
        _udpclient = udpclient;
        this.server = server;
    }

    public void Send(byte[] data)
    {
        server.Send(data, data.Length, _udpclient);
    }

    [Obsolete("Shutdown a UDP client is meaningless")]
    public void Disconnect()
    {
        
    }
}

public class PCMUDDSlient : IComClient
{
    [Obsolete("Check alive of UDP Client is meaningless.")]
    public bool IsAlive { get; set; }

    public IPEndPoint Client => _udsclient;

    private IPEndPoint _udsclient;

    public PCMUDDSlient(IPEndPoint udsclient)
    {
        _udsclient = udsclient;
    }

    [Obsolete("Shutdown a UDP client is meaningless")]
    public void Disconnect()
    {

    }
}


