using BananaBoard.Hardware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BananaBoard.Controls;
public sealed class LCDSimpleProgressBar : LCDControl
{
    public LCDSimpleProgressBar(SPILCD dev) : base(dev)
    {
        Size = new System.Drawing.Size(100, 20);
        BarMargin = new Margins(0, 0, 0, 0);
    }

    public int MaxValue { get; set; } = 100;

    int value = 0;
    int? oldvalue = null;
    bool valuechange = false;
    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            if(IsShouldAutorefresh)
            {
                valuechange = true;
                Refresh();
            }
        }
    }

    Margins margin;
    Size Realsize => new Size(Size.Width - margin.Left - margin.Right, Size.Height - margin.Top - margin.Bottom);
    Point Realpos => new Point(AbsolutePosition.X + margin.Left, AbsolutePosition.Y + margin.Top);
    public Margins BarMargin
    {
        get
        {
            return margin;
        }
        set
        {
            margin = value;

        }
    }

    public override void Refresh()
    {
        if (AutoCoverOldArea && !valuechange)
            base.CoverOldArea(new System.Drawing.Rectangle(AbsolutePosition, @Size));

        var realsize = Realsize;
        var realpos = Realpos;
        valuechange = false;
        int fill_width = (int)(realsize.Width * value / (float)MaxValue);

        if(oldvalue == null)
        {
            Device.UI_FillRec((ushort)AbsolutePosition.X, (ushort)AbsolutePosition.Y, (ushort)Size.Width, (ushort)Size.Height, BackColor);
        }
        
        Device.UI_FillRec((ushort)realpos.X, (ushort)realpos.Y, (ushort)fill_width, (ushort)realsize.Height, ForeColor);
        if(oldvalue >= value)
        {
            int void_width = Size.Width - fill_width;
            Device.UI_FillRec((ushort)(AbsolutePosition.X + fill_width), (ushort)AbsolutePosition.Y, (ushort)void_width, (ushort)Size.Height, BackColor);     
        }
        oldvalue = value;
    }
}
