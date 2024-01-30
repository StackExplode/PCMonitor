using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

namespace BananaBoard.Hardware;

public delegate void OnTempRHDataDlg(double temp, double rh);
internal class TempSensor : IDisposable
{
    SerialPort port = new SerialPort();
    private bool disposedValue;

    public event OnTempRHDataDlg OnTempRHData;

    public TempSensor(string name, int rate) 
    { 
        port.PortName = name;
        port.BaudRate = rate;
        port.DataReceived += Port_DataReceived;
    }

    private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        string s = port.ReadExisting();
        Console.WriteLine("Rec:{0}", s);
        
    }

    public string PortName
    {
        get => port.PortName;
        set => port.PortName = value;
    }

    public int BaudRate
    {
        get => port.BaudRate;
        set => port.BaudRate = value;
    }

    public void Start()
    {
        if (!port.IsOpen)
            port.Open();
    }

    public void Stop()
    {
        if(port.IsOpen)
            port.Close();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                port.Dispose();
                // TODO: 释放托管状态(托管对象)
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~TempSensor()
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
