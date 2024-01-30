using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using PCMonitorCommon.Driver;
using System.Net;
using PCMonitorCommon.Entity;

namespace PCMonitorRouter
{
    internal class Program
    {

        static UDPDriver udpserver;

        static void Main(string[] args)
        {

            Console.WriteLine("Program started...");

            AppDomain.CurrentDomain.ProcessExit += (a, b) => Clean();
            Console.CancelKeyPress += (_, _) => Clean();


            udpserver = new UDPDriver();
            udpserver.SetParameter(IPAddress.Any, 17877);
            udpserver.Init();
            udpserver.OnComDataReceived += Udpserver_OnComDataReceived;
            udpserver.Start();

            



            Console.ReadLine();
            Console.WriteLine("Program finished");
            
        }

        private static void Udpserver_OnComDataReceived(IComClient client, byte[] data)
        {
            const int dd = 9, dd2 = 3;
            if (data[0] == 0x81)
            {
                Console.WriteLine("UDP data rec!");
                SerialPort sp = new SerialPort("/dev/ttyS1", 9600);
                sp.Open();
                sp.WriteLine($"S1D{dd:000}T");
                Thread.Sleep(200);
                sp.WriteLine($"S1D000T");
                Thread.Sleep(3000);
                sp.WriteLine($"S1D{dd2:000}T");
                sp.Close();
            }
        }

        static void Clean()
        {
            udpserver.Stop();
            udpserver.Dispose();
            Console.WriteLine("Clean Finished!");
        }
    }

    
}
