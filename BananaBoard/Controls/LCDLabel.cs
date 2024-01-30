using BananaBoard.Hardware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BananaBoard.Controls;
public sealed class LCDLabel : LCDControl
{
    public LCDLabel(SPILCD dev) : base(dev)
    {
        @Font = new Font("Microsoft YaHei UI", 8);
        BackColor = Color.Transparent;
    }



    string text = "label";
    public string Text 
    { 
        get
        {
            return text;
        }
        set 
        { 
            text = value;
            if (base.IsShouldAutorefresh)
                this.Refresh();
        } 
    }

    private Font font;
    public Font @Font
    {
        get
        {
            return font;
        }
        set
        {
            font = value;
            if (IsShouldAutorefresh)
                this.Refresh();
        }
    }
   
    //public bool RightToLeft { get; set; } = false;
    public bool AutoSize { get; set; } = true;
    

   
    public Size RealSize
    {
        get
        {
            if (AutoSize)
                return Device.graphics.MeasureString(text, Font).ToSize();
            else
                return this.Size;
        }
    }

    

    public override void Refresh()
    {
        

        SizeF string_size;
        if (AutoSize)
            string_size = this.RealSize;
        else
            string_size = this.Size;

        int w = 0;
        int h = 0;

        Graphics g = Device.graphics;

        //string_size = g.MeasureString(text, Font);

        w = (int)Math.Ceiling(string_size.Width);
        h = (int)Math.Ceiling(string_size.Height);

        Rectangle ImageSize = new Rectangle(AbsolutePosition.X, AbsolutePosition.Y, w, h);

        if (AutoCoverOldArea)
            base.CoverOldArea(ImageSize);

        g.FillRectangle(new SolidBrush(BackColor), ImageSize);

        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

        RectangleF rectf = new RectangleF(AbsolutePosition.X, AbsolutePosition.Y, string_size.Width, string_size.Height);
        g.DrawString(text, @Font, new SolidBrush(ForeColor), rectf);
        g.Flush();

        Device.FlushDisplay(ImageSize);

        
    }
}
