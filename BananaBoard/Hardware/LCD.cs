using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using static BananaBoard.Hardware.WiringPi;
using static BananaBoard.Hardware.WiringPiSPI;
using u8 = System.Byte;
using u16 = System.UInt16;
using System.Drawing;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace BananaBoard.Hardware;
public class SPILCD : IDisposable
{
    private const int SPI_CHANNEL = 0;
    private const int BG_PIN = 23;
    private const int DC_PIN = 24;
    private const int RST_PIN = 25;
    public const int LCD_W = 320;
    public const int LCD_H = 480;

    int fd = 0;

    private _lcd_dev_type lcddev = new _lcd_dev_type();
    private u16 DeviceCode;


    private Color fcolor = Color.White, bcolor = Color.Black;
    private u16 BACK_COLOR, POINT_COLOR;

    public Color ForeColor
    {
        get
        {
            return fcolor;
        }
        set
        {
            fcolor = value;
            POINT_COLOR = Color16.ConvertColorTo16(fcolor);
        }
    }
    public Color BackColor
    {
        get
        {
            return bcolor;
        }
        set
        {
            bcolor = value;
            BACK_COLOR = Color16.ConvertColorTo16(bcolor);
        }
    }


    private static FileStream devFile;
    private bool disposedValue;
    private Dictionary<LCDFont, FontInfo> support_fonts = new Dictionary<LCDFont, FontInfo>();

    private bool bg = true;
    public bool BackLight
    {
        get
        {
            return bg;
        }
        set
        {
            bg = value;
            digitalWrite(BG_PIN, bg ? HIGH : LOW);
        }
    }

    public void Init(int speed)
    {
        support_fonts.Add(LCDFont.Font_16x32, new FontInfo(16,32,64,LCDFonts.CharTbl32x16));
        support_fonts.Add(LCDFont.Font_8x16, new FontInfo(8, 16,16, LCDFonts.CharTbl16x8));
        support_fonts.Add(LCDFont.Font_12x16, new FontInfo(12, 16,32, LCDFonts.CharTbl12x16));
        support_fonts.Add(LCDFont.Font_12x24, new FontInfo(12, 24,48, LCDFonts.CharTbl24x12));

        if (wiringPiSetup() < 0)
            throw new Exception("Init wiringPi error!");
        fd = wiringPiSPISetupMode(SPI_CHANNEL, speed, 0);
        if (fd < 0)
            throw new Exception("Init SPI failed!");

        pinMode(DC_PIN, OUTPUT);
        pinMode(RST_PIN, OUTPUT);
        pinMode(BG_PIN, OUTPUT);
        digitalWrite(RST_PIN, HIGH);
        digitalWrite(BG_PIN, HIGH);


        devFile = File.Open("/dev/spidev0.0", FileMode.Open);
        

        LCD_InitCommands();
        InitTTFDrawing();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void LCD_Cmd_Mode() => digitalWrite(DC_PIN, LOW);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void LCD_Data_Mode() => digitalWrite(DC_PIN, HIGH);

#if false
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SPI_WriteByte(byte data) => wiringPiSPIDataRW(SPI_CHANNEL, new byte[]{data}, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SPI_WriteBytes(byte[] data)
    {
        const int batch = 2048;
        int max = data.Length / batch;
        int rest = data.Length % batch;
        for(int i = 0; i < max; i++)
        {

        }
        wiringPiSPIDataRW(SPI_CHANNEL, data, data.Length);
    }
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SPI_WriteByte(byte data)
    {
        devFile.WriteByte(data);
        devFile.Flush();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SPI_WriteBytes(byte[] data)
    {
        const int batch = 2048;
        int max = data.Length / batch;
        int rest = data.Length % batch;
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        for (int i = 0; i < max; i++)
        {
            devFile.Write(data, i * batch, batch);
        }
        if (rest > 0)
            devFile.Write(data, max * batch, rest);
        devFile.Flush();
        //sw.Stop();
        LCD_Cmd_Mode();
        //Console.WriteLine($"Send {data.Length} bytes data, using {sw.ElapsedMilliseconds} ms");
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void LCD_WriteReg(u8 LCD_Reg, u8 LCD_RegValue)
    {
        LCD_Cmd_Mode();
        SPI_WriteByte(LCD_Reg);
        LCD_Data_Mode();
        SPI_WriteByte(LCD_RegValue);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void LCD_WriteRAM_Prepare()
    {
        LCD_Cmd_Mode();
        SPI_WriteByte(lcddev.wramcmd);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Lcd_WritePixel_16Bit(u16 Data)
    {
        LCD_Data_Mode();
        u8 R = (byte)((Data >> 8) & 0xF8);
        u8 G = (byte)((Data >> 3) & 0xFC);
        u8 B = (byte)(Data << 3);
        byte[] buffer = { R, G, B };
        SPI_WriteBytes(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Lcd_WritePixel_16Bit(Color color)
    {
        LCD_Data_Mode();
        Color16 c = default;
        c.FromColor(color);
        byte[] buffer = { c.R, c.G, c.B };
        SPI_WriteBytes(buffer);
    }

    public void LCD_SetWindows(u16 xStar, u16 yStar, u16 xEnd, u16 yEnd)
    {
        LCD_Cmd_Mode();
        SPI_WriteByte(lcddev.setxcmd);
        LCD_Data_Mode();
        byte[] buffer1 =
        {
            (byte)(xStar >> 8),
            (byte)(0x00FF & xStar),
            (byte)(xEnd >> 8),
            (byte)(0x00FF & xEnd)
        };
        SPI_WriteBytes(buffer1);

        LCD_Cmd_Mode();
        SPI_WriteByte(lcddev.setycmd);
        LCD_Data_Mode();
        byte[] buffer2 =
        {
            (byte)(yStar >> 8),
            (byte)(0x00FF & yStar),
            (byte)(yEnd >> 8),
            (byte)(0x00FF & yEnd)
        };
        SPI_WriteBytes(buffer2);

        LCD_WriteRAM_Prepare(); //开始写入GRAM			
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LCD_SetCursor(u16 Xpos, u16 Ypos)
    {
        LCD_SetWindows(Xpos, Ypos, Xpos, Ypos);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LCD_DrawPoint(u16 x, u16 y,u16 cl)
    {
        LCD_SetCursor(x, y);//设置光标位置 
        Lcd_WritePixel_16Bit(cl);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LCD_DrawPoint(u16 x, u16 y, Color cl)
    {
        LCD_SetCursor(x, y);//设置光标位置 
        Lcd_WritePixel_16Bit(cl);
    }

    public void LCD_Clear(Color cl)
    {
        //u16 cc = Color16.ConvertColorTo16(c);
        uint i, m;
        LCD_SetWindows(0, 0, (ushort)(lcddev.width - 1), (ushort)(lcddev.height - 1));
        LCD_Data_Mode();
        BackColor = cl;
        LCD_SetWindows(0, 0, (ushort)(lcddev.width - 1), (ushort)(lcddev.height - 1));
        int max = lcddev.width * lcddev.height;
        byte[] buffer = new byte[max * 3];
        Span<Color16> rgb = MemoryMarshal.Cast<byte, Color16>(buffer.AsSpan());
        Color16 c = default;
        c.FromColor(cl);
        rgb.Fill(c);
        LCD_Data_Mode();
        SPI_WriteBytes(buffer);

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void delay_ms(int ms) => System.Threading.Thread.Sleep(ms);



    private void LCD_RESET()
    {
        digitalWrite(RST_PIN, LOW);
        delay_ms(100);
        digitalWrite(RST_PIN, HIGH);
        delay_ms(50);
    }

    public void LCD_SetDirection(LCD_Direction dir)
    {
        u8 direction = (byte)dir;

        lcddev.setxcmd = 0x2A;
        lcddev.setycmd = 0x2B;
        lcddev.wramcmd = 0x2C;
        switch (direction)
        {
            case 0:
                lcddev.width = LCD_W;
                lcddev.height = LCD_H;
                LCD_WriteReg(0x36, (1 << 3) | (0 << 6) | (0 << 7));//BGR==1,MY==0,MX==0,MV==0
                break;
            case 1:
                lcddev.width = LCD_H;
                lcddev.height = LCD_W;
                LCD_WriteReg(0x36, (1 << 3) | (0 << 7) | (1 << 6) | (1 << 5));//BGR==1,MY==1,MX==0,MV==1
                break;
            case 2:
                lcddev.width = LCD_W;
                lcddev.height = LCD_H;
                LCD_WriteReg(0x36, (1 << 3) | (1 << 6) | (1 << 7));//BGR==1,MY==0,MX==0,MV==0
                break;
            case 3:
                lcddev.width = LCD_H;
                lcddev.height = LCD_W;
                LCD_WriteReg(0x36, (1 << 3) | (1 << 7) | (1 << 5));//BGR==1,MY==1,MX==0,MV==1
                break;
            default: break;
        }
    }


    void LCD_InitCommands()
    {
        var LCD_WR_DATA = (u8 data) => { LCD_Data_Mode();SPI_WriteByte(data); };
        var LCD_WR_REG = (u8 data) => { LCD_Cmd_Mode(); SPI_WriteByte(data); };

        LCD_RESET();

        LCD_WR_REG(0XF7);
        LCD_WR_DATA(0xA9);
        LCD_WR_DATA(0x51);
        LCD_WR_DATA(0x2C);
        LCD_WR_DATA(0x82);
        LCD_WR_REG(0xC0);
        LCD_WR_DATA(0x11);
        LCD_WR_DATA(0x09);
        LCD_WR_REG(0xC1);
        LCD_WR_DATA(0x41);
        LCD_WR_REG(0XC5);
        LCD_WR_DATA(0x00);
        LCD_WR_DATA(0x0A);
        LCD_WR_DATA(0x80);
        LCD_WR_REG(0xB1);
        LCD_WR_DATA(0xB0);
        LCD_WR_DATA(0x11);
        LCD_WR_REG(0xB4);
        LCD_WR_DATA(0x02);
        LCD_WR_REG(0xB6);
        LCD_WR_DATA(0x02);
        LCD_WR_DATA(0x42);
        LCD_WR_REG(0xB7);
        LCD_WR_DATA(0xc6);
        LCD_WR_REG(0xBE);
        LCD_WR_DATA(0x00);
        LCD_WR_DATA(0x04);
        LCD_WR_REG(0xE9);
        LCD_WR_DATA(0x00);
        LCD_WR_REG(0x36);
        LCD_WR_DATA((1 << 3) | (0 << 7) | (1 << 6) | (1 << 5));
        LCD_WR_REG(0x3A);
        LCD_WR_DATA(0x66);
        LCD_WR_REG(0xE0);
        LCD_WR_DATA(0x00);
        LCD_WR_DATA(0x07);
        LCD_WR_DATA(0x10);
        LCD_WR_DATA(0x09);
        LCD_WR_DATA(0x17);
        LCD_WR_DATA(0x0B);
        LCD_WR_DATA(0x41);
        LCD_WR_DATA(0x89);
        LCD_WR_DATA(0x4B);
        LCD_WR_DATA(0x0A);
        LCD_WR_DATA(0x0C);
        LCD_WR_DATA(0x0E);
        LCD_WR_DATA(0x18);
        LCD_WR_DATA(0x1B);
        LCD_WR_DATA(0x0F);
        LCD_WR_REG(0XE1);
        LCD_WR_DATA(0x00);
        LCD_WR_DATA(0x17);
        LCD_WR_DATA(0x1A);
        LCD_WR_DATA(0x04);
        LCD_WR_DATA(0x0E);
        LCD_WR_DATA(0x06);
        LCD_WR_DATA(0x2F);
        LCD_WR_DATA(0x45);
        LCD_WR_DATA(0x43);
        LCD_WR_DATA(0x02);
        LCD_WR_DATA(0x0A);
        LCD_WR_DATA(0x09);
        LCD_WR_DATA(0x32);
        LCD_WR_DATA(0x36);
        LCD_WR_DATA(0x0F);
        LCD_WR_REG(0x11);
        delay_ms(120);
        LCD_WR_REG(0x29);

        LCD_SetDirection(LCD_Direction.Rotate_0);

        
    }

    public void UI_FillRec(u16 x, u16 y, u16 width, u16 height, Color cl)
    {
        LCD_SetWindows(x, y, (ushort)(x + width - 1), (ushort)(y + height - 1));
        int max = width * height ;
        byte[] buffer = new byte[max*3];
        Span<Color16> rgb = MemoryMarshal.Cast<byte, Color16>(buffer.AsSpan());
        Color16 c = default;
        c.FromColor(cl);
        rgb.Fill(c);
        LCD_Data_Mode();
        SPI_WriteBytes(buffer);
    }

    public void UI_DrawChar(u16 x, u16 y, char ch, LCDFont font, Color fcolor, Color bcolor)
    {
        var info = support_fonts[font];
        int b_count = info.Width * info.Height / 8;
        int index = (ch - 0x20) * info.Size;
        var span = info.FontData.AsSpan().Slice(index, info.Size);
        Color16 fc = default;
        Color16 bc = default;
        fc.FromColor(fcolor);
        bc.FromColor(bcolor);
        byte[] buffer = new byte[b_count * 8 * 3];
        int k = 0;
        //Console.WriteLine($"{info.Size},{b_count},{info.Width},{info.Height}");
        byte mod = (byte)(info.Width % 8);
        for (int i = 0; i < info.Size; i++)
        {
            for (u8 j = 0; j < (8 - (i % 2) * mod); j++)
            {
                if ((span[i] & (1 << j)) != 0)
                {
                    buffer[k++] = fc.R;
                    buffer[k++] = fc.G;
                    buffer[k++] = fc.B;
                }
                else
                {
                    buffer[k++] = bc.R;
                    buffer[k++] = bc.G;
                    buffer[k++] = bc.B;
                }
            }
        }
        LCD_SetWindows(x, y, (ushort)(x + info.Width - 1), (ushort)(y + info.Height - 1));
        LCD_Data_Mode();
        SPI_WriteBytes(buffer);
    }

    public void UI_DrawChar(u16 x, u16 y, char ch, LCDFont font) => UI_DrawChar(x, y, ch, font, ForeColor, BackColor);

    public void UI_DrawString(u16 x, u16 y, string s, LCDFont font, Color fc, Color bc)
    {
        int k = 0;
        while (k < s.Length)
        {
            UI_DrawChar(x, y, s[k], font, fc, bc);
            k++;
            x += (u16)support_fonts[font].Width;
        }
    }

    public void UI_DrawString(u16 x, u16 y, string s, LCDFont font) => UI_DrawString(x, y, s, font, ForeColor, BackColor);

    Bitmap _bmp;
    Graphics _g;
    public Graphics graphics => _g;
    public Bitmap @Image => _bmp;
    private void InitTTFDrawing()
    {
        _bmp = new Bitmap(LCD_W, LCD_H, PixelFormat.Format16bppRgb565);
        _g = Graphics.FromImage(_bmp);
        Graphics g = _g;
        SizeF string_size = default;
        int w = 0;
        int h = 0;
        var f = new Font("Consolas", 13);
        string_size = g.MeasureString("A", f);

        w = (int)Math.Ceiling(string_size.Width);
        h = (int)Math.Ceiling(string_size.Height);

        Rectangle ImageSize = new Rectangle(0, 0, w, h);
        g.FillRectangle(new SolidBrush(Color.Green), ImageSize);

        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

        RectangleF rectf = new RectangleF(0, 0, string_size.Width, string_size.Height);
        g.DrawString("A", f, new SolidBrush(Color.Blue), rectf);
        g.Flush();
    }

    public Size UI_DrawTTFString(ushort x, ushort y, string s, Font font, Color fc, Color bc)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
#pragma warning disable CA1416
       
        SizeF string_size = default;
        int w = 0;
        int h = 0;

        Graphics g = _g;
                    
        string_size = g.MeasureString(s, font);
            
        w = (int)Math.Ceiling(string_size.Width);
        h = (int)Math.Ceiling(string_size.Height);
            
        Rectangle ImageSize = new Rectangle(x, y, w, h);
        g.FillRectangle(new SolidBrush(bc), ImageSize);
            
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            
        RectangleF rectf = new RectangleF(x, y, string_size.Width, string_size.Height);
        g.DrawString(s, font, new SolidBrush(fc), rectf);
        g.Flush();


        FlushDisplay(ImageSize);
        

#pragma warning restore CA1416
        sw.Stop();
        Console.WriteLine("Total cost {0}ms", sw.ElapsedMilliseconds);
        return new Size(w, h);

    }

    public Size UI_DrawTTFString(ushort x, ushort y, string s, Font font) => UI_DrawTTFString(x, y, s, font, ForeColor, BackColor);

    public unsafe void FlushDisplay(Rectangle area)
    {
        BitmapData data = _bmp.LockBits(area, ImageLockMode.ReadWrite, _bmp.PixelFormat);

        byte* ptr = (byte*)data.Scan0;
        byte[] buffer = new u8[data.Width * data.Height * 3];

        const int COLOR_CHANNEL = 4;//BGRA
        fixed (byte* pp = buffer)
        {
            byte* p = pp;
            for (int j = 0; j < data.Height; j++)
            {
                byte* scanPtr = ptr + (j * data.Stride);
                for (int i = 0; i < data.Width; i++, scanPtr += COLOR_CHANNEL)
                {
                    *(p + 2) = (byte)((*ptr++) & 0xF8);
                    *(p + 1) = (byte)((*ptr++) & 0xFC);
                    *(p + 0) = (byte)((*ptr++) & 0xF8);
                    ptr++;
                    p += 3;
                }
            }
        }

        _bmp.UnlockBits(data);

        u16 x = (ushort)area.Left;
        u16 y = (ushort)area.Top;
        LCD_SetWindows(x, y, (ushort)(x + area.Width - 1), (ushort)(y + area.Height - 1));
        LCD_Data_Mode();
        SPI_WriteBytes(buffer);
    }

    public void UI_DrawRectangle(u16 x, u16 y, u16 width, u16 height, u8 thickness,Color cl)
    {
        UI_FillRec(x, y, width, thickness, cl);
        UI_FillRec(x, y, thickness, height , cl);
        UI_FillRec((ushort)(x + width), y, thickness, (ushort)(height + thickness), cl);
        UI_FillRec(x, (ushort)(y + height), width, thickness, cl);
    }

    public void TestDraw(int x)
    {
        //         var rect = new Rectangle(100, 100, 50, 50);
        //         graphics.FillEllipse(Brushes.White,rect);
        //         graphics.FillPie(Brushes.Red, rect, -90, x);
        //         graphics.Flush();
        //         FlushDisplay(rect);
        var img = Bitmap.FromFile("cpu.png");
       // var img = new Bitmap(img0, new Size(64, 64));
       
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _g.Dispose();
                devFile.Dispose();
                WiringPi.close(fd);
                // TODO: 释放托管状态(托管对象)
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~SPILCD()
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

internal class _lcd_dev_type
{
    public u16 width;          //LCD 宽度
    public u16 height;         //LCD 高度
    public u16 id;               //LCD ID
    public u8  dir;           //横屏还是竖屏控制：0，竖屏；1，横屏。	
    public u8 wramcmd;        //开始写gram指令
    public u8 setxcmd;        //设置x坐标指令
    public u8 setycmd;		//设置y坐标指令
}

public enum LCD_Direction : byte
{
    Rotate_0 = 0,
    Rotate_90 = 1,
    Rotate_180 = 2,
    Rotate_270 = 3,
}

internal struct Color16
{
    public void FromRBG16Data(u16 color)
    {
        R = (byte)((color >> 8) & 0xF8);
        G = (byte)((color >> 3) & 0xFC);
        B = (byte)(color << 3);
    }
    public void FromColor(Color c)
    {
        R = (byte)(c.R & 0xF8);
        G = (byte)(c.G & 0xFC);
        B= (byte)(c.B & 0xF8);

    }
    public u8 R;
    public u8 G;
    public u8 B;

    public u16 ToRGB16Data()
    {
        return ((u16)((((R) << 8) & 0xF800) | (((G) << 3) & 0x7E0) | ((B) >> 3)));
    }

    public static u16 ConvertColorTo16(Color c)
    {
        return ((u16)((((c.R) << 8) & 0xF800) | (((c.G) << 3) & 0x7E0) | ((c.B) >> 3)));
    }
}

internal struct FontInfo
{
    public FontInfo(int w, int h,int s, byte[] arr)
    {
        Width=w; 
        Height=h;
        Size = s;
        FontData = arr;
    }
    public int Width;
    public int Height;
    public int Size;
    public byte[] FontData;
}

public enum LCDFont
{
    Font_16x32,
    Font_8x16,
    Font_12x24,
    Font_12x16,
    Big = Font_16x32,
    Middle = Font_12x24,
    Small = Font_8x16,
    Strange = Font_12x16
}
