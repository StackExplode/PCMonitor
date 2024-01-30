using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using BananaBoard.Controls;
using BananaBoard.Hardware;
using PCMonitorCommon.Driver;
using static BananaBoard.Hardware.WiringPi;

namespace BananaBoard
{

    internal class Program
    {

        //00:90:9e:9a:a1:0c
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SPILCD lcd = new SPILCD();
            lcd.Init(32*1000*1000);
            lcd.LCD_SetDirection(LCD_Direction.Rotate_0);
            lcd.LCD_Clear(Color.Black);

            Form_CPU cpu = new Form_CPU(lcd);
            cpu.Show();

            Form_GPU gpu = new Form_GPU(lcd);
            gpu.Show();

            Console.ReadLine();
        }

        static void TestDrawLCD(SPILCD lcd)
        {
            Color cl = Color.FromArgb(0x102cff);
            lcd.UI_FillRec(0, 0, SPILCD.LCD_W, 64 + 3, cl);
            lcd.UI_DrawString(4 * 16, 0, "32", LCDFont.Big, Color.Yellow, cl);
            lcd.UI_DrawString(6 * 16, 8, "%", LCDFont.Middle, Color.LightGray, cl);
            lcd.UI_DrawString(6 * 16 + 1 * 12, 0, " 085", LCDFont.Big, Color.White, cl);
            lcd.UI_DrawString(10 * 16 + 1 * 12 + 4, 8, "W", LCDFont.Middle, Color.LightGray, cl);
            lcd.UI_DrawString(10 * 16 + 2 * 12 + 4, 0, " 1.25", LCDFont.Big, Color.White, cl);
            lcd.UI_DrawString(15 * 16 + 2 * 12 + 8, 8, "V", LCDFont.Middle, Color.LightGray, cl);

            lcd.UI_DrawString(0, 32, "65", LCDFont.Big, Color.Red, cl);
            lcd.UI_DrawString(2 * 16 + 8, 32, "C ", LCDFont.Big, Color.LightGray, cl);
            lcd.UI_DrawString(32 + 3, 28, "o", LCDFont.Small, Color.LightGray, cl);
            lcd.UI_DrawString(0, 0, "CPU:", LCDFont.Big, cl, Color.White);

            lcd.UI_DrawString(4 * 16 + 8, 32, "5.1", LCDFont.Big, Color.FromArgb(0xff6b88), cl);
            lcd.UI_DrawString(7 * 16 + 8, 32, "/", LCDFont.Big, Color.LightGray, cl);
            lcd.UI_DrawString(8 * 16 + 8, 32, "3.6", LCDFont.Big, Color.FromArgb(0x00FFFF), cl);
            lcd.UI_DrawString(11 * 16 + 16, 40, "GHz  ", LCDFont.Middle, Color.LightGray, cl);
            lcd.UI_DrawString(11 * 16 + 5 * 12 + 16, 40, "00/12", LCDFont.Middle, Color.White, cl);
        }
    }
}
