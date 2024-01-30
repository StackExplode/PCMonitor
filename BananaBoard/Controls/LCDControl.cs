using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BananaBoard.Hardware;

namespace BananaBoard.Controls;
public abstract class LCDControl
{
    public LCDControl(SPILCD dev)
    {
        Device = dev;
        Position = new Point(0, 0);
    }

    protected virtual SPILCD Device { get; init; }

    protected Rectangle? oldarea = null;
    public Rectangle OldArea => oldarea ?? new Rectangle();
    public bool AutoCoverOldArea { get; set; } = true;
    public string Name { get; set; }

    private Point position;
    public Point AbsolutePosition 
    {
        get
        {
            var (x, y) = GetParentAPosition();
            return new Point(position.X + x, position.Y + y);
        }
    }

    private (int x, int y) GetParentAPosition()
    {
        if (Parent is null)
            return (0, 0);
        else
            return (Parent.AbsolutePosition.X, Parent.AbsolutePosition.Y);
    }
    public virtual Point Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
           if (IsShouldAutorefresh)
                this.Refresh();
        }
    }

    protected Size size;
    public virtual Size Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
            if (IsShouldAutorefresh)
                this.Refresh();
        }
    }

    private Color fcolor = Color.White, bcolor = Color.Black;
    public Color BackColor
    {
        get
        {
            return bcolor;
        }
        set
        {
            bcolor = value;
            if (IsShouldAutorefresh)
                this.Refresh();
        }
    }
    public Color ForeColor
    {
        get
        {
            return fcolor;
        }
        set
        {
            fcolor = value;
            if (IsShouldAutorefresh)
                this.Refresh();
        }
    }

    public virtual bool AutoRefresh { get; set; } = true;
    protected bool IsShouldAutorefresh
    {
        get
        {
            return (Parent?.IsShowing ?? true) && this.IsShowing && this.AutoRefresh;
        }
    }

    public LCDControlContainer Parent { get; set; }

    protected bool isshowing = false;
    public bool IsShowing => isshowing;

    public virtual void Hide()
    {
        if(oldarea is not null)
            CoverOldArea((Rectangle)oldarea);
        isshowing = false;
    }

    public virtual void Show()
    {
        Refresh();
        isshowing = true;
    }

    public abstract void Refresh();

    public virtual void CoverOldArea(Rectangle newarea)
    {
        Device.UI_FillRec((ushort)OldArea.X, (ushort)OldArea.Y, (ushort)OldArea.Width, (ushort)OldArea.Height, Parent?.BackColor ?? Device.BackColor);
        oldarea = newarea;
    }
}
