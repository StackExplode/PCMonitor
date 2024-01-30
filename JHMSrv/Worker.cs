using Microsoft.Extensions.Hosting;
using PCMonitorCommon.Driver;
using PCMonitorCommon.Entity;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace JHMSrv;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly EventLog mylogger;

    private readonly IHostApplicationLifetime lifetime;


    public Worker(ILogger<Worker> logger, IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        lifetime = hostApplicationLifetime;
        mylogger = new EventLog("JMHSrv");
        mylogger.Source = "JMHSrv";
       
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            UDPDriver udpserver = new UDPDriver();
            udpserver.SetParameter(IPAddress.Any, 17878);
            udpserver.Init();
            udpserver.OnComDataReceived += Udpserver_OnComDataReceived;
            udpserver.Start();

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException) { }
            

            udpserver.Stop();
            udpserver.Dispose();
        }
        catch (Exception ex)
        {
            mylogger.WriteEntry("Error occured: " + ex.Message, EventLogEntryType.Error);
        }
        finally
        {
            mylogger.WriteEntry("My service stopped!", EventLogEntryType.Information);
            mylogger.Dispose();
            lifetime.StopApplication();
        }
        
    }

    private void Udpserver_OnComDataReceived(IComClient client, byte[] data)
    {
        PCMUDPClient cl = client as PCMUDPClient;
        string exe_file = "";
        switch(data[0])
        {
            case 1: exe_file = "C:/windows/notepad.exe"; break;
            case 2: exe_file = "E:\\AnyDesk.exe"; break;
            case 3: exe_file = "E:\\RustDesk.exe"; break;

            default: cl.Send(new byte[] { 0x02 });return;
        }

        uint rt = ApplicationLoader.StartProcessAndBypassUAC(exe_file, out _);
        if (rt != 0)
        {
            mylogger.WriteEntry($"Create process failed. Error code={rt}", EventLogEntryType.Warning);
            cl.Send(new byte[] { 0x01 });
        }
        else
        {
            mylogger.WriteEntry($"Create process success! Path={exe_file}", EventLogEntryType.Information);
            cl.Send(new byte[] { 0x00 });
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        mylogger.WriteEntry("My service started!", EventLogEntryType.Information);
        
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        mylogger.WriteEntry("My service is about stop!", EventLogEntryType.Information);
        //mylogger.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
