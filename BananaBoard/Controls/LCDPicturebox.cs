using BananaBoard.Hardware;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BananaBoard.Controls;
public sealed class LCDPicturebox : LCDControl
{
    public LCDPicturebox(SPILCD dev) : base(dev)
    {
        Size = new Size(100, 100);
        BackColor = Color.Transparent;
    }

    Image img = null;
    public Image @Image
    {
        get => img;
        set
        {
            img = value;
            if(IsShouldAutorefresh)
                Refresh();
        }
    }

    public ImageStretchType StretchType { get; set; } = ImageStretchType.None;

    public override void Refresh()
    {
        if (Image == null)
            return;

        Rectangle rect;
        if(StretchType == ImageStretchType.Fit) 
            rect = new Rectangle(AbsolutePosition.X, AbsolutePosition.Y, img.Width, img.Height);
        else
            rect = new Rectangle(AbsolutePosition.X, AbsolutePosition.Y, @Size.Width, @Size.Height);
        var g = Device.graphics;

        if (AutoCoverOldArea)
            base.CoverOldArea(rect);


        g.SmoothingMode = SmoothingMode.None;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

        g.FillRectangle(new SolidBrush(BackColor), rect);
        if (StretchType == ImageStretchType.Stretch)
            g.DrawImage(img, rect.X, rect.Y, @Size.Width, @Size.Height);
        else
            g.DrawImage(img, rect.X, rect.Y);
        g.Flush();
        Device.FlushDisplay(rect);
    }
}

public enum ImageStretchType
{
    None,
    Stretch,
    Fit
}
