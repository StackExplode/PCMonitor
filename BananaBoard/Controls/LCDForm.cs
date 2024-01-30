using BananaBoard.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace BananaBoard.Controls;
public abstract class LCDForm : IDisposable, LCDControlContainer
{
    SPILCD _lcd;
    protected SPILCD Device => _lcd;
    Channel<Action> ch;
    Task task;
    bool _running = true;
    CancellationTokenSource ch_cancel;
    Semaphore locker;

    bool isshowing = false;
    public bool IsShowing => isshowing;
    LCDControlCollection controls;
    public LCDControlCollection Controls => controls;

    public Color BackColor { get; set; }
    public Size @Size { get; set; }
    public Point Position { get; set; }

    public LCDForm(SPILCD dev)
    {
        _lcd = dev;
        controls = new LCDControlCollection(this);
        BackColor = dev.BackColor;   
        ch = Channel.CreateBounded<Action>(100);
        ch_cancel = new CancellationTokenSource();
        locker = new Semaphore(0, 100);
        
        task = Task.Factory.Create(() =>
        {
            do
            {
                locker.WaitOne();
                ch.Reader.WaitToReadAsync(ch_cancel.Token);
                Action act;
                bool b = ch.Reader.TryRead(out act);
                if (b)
                    act.Invoke();
            } while (_running);
        });
        task.Start();
    }

    public void Invoke(Action act)
    {
        ch.Writer.TryWrite(act);
        locker.Release();
    }

    public virtual void Show()
    {
        Device.UI_FillRec((ushort)Position.X, (ushort)Position.Y, (ushort)Size.Width, (ushort)Size.Height, this.BackColor);
        foreach(var c in controls)
        {
            c.Show();
        }
        isshowing = true;
    }

    public virtual void Hide()
    {
        Device.UI_FillRec((ushort)Position.X, (ushort)Position.Y, (ushort)Size.Width, (ushort)Size.Height,Device.BackColor);
        isshowing = false;
    }

    public void Refresh()
    {
        this.Hide();
        this.Show();
    }

    private bool disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _running = false;
                locker.Close();
                locker.Dispose();
                ch.Writer.Complete();
                ch_cancel.Cancel();
                // TODO: 释放托管状态(托管对象)
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~PCMonitorUI()
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

public interface LCDControlContainer
{
    public LCDControlCollection Controls { get; }
    public Color BackColor { get; set; }
    public Size @Size { get; set; }
    public Point Position { get; set; }
    public bool IsShowing { get; }
    public Point AbsolutePosition => Position;
}

public class LCDControlCollection : ICollection<LCDControl>
{
    LCDControlContainer parent;
    public LCDControlCollection(LCDControlContainer p)
    {
        parent = p;
    }

    private List<LCDControl> list = new List<LCDControl>();
    public int Count => list.Count;

    public bool IsReadOnly => true;

    public void Add(LCDControl item)
    {
        list.Add(item);
        item.Parent = parent;
    }

    public void Clear()
    {

        foreach (LCDControl item in list)
            item.Parent = null;
        list.Clear();
    }

    public bool Contains(LCDControl item)
    {
        return list.Contains(item);
    }

    public void CopyTo(LCDControl[] array, int arrayIndex)
    {
        list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<LCDControl> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    public bool Remove(LCDControl item)
    {
        bool rst = list.Remove(item);
        if(rst)
            item.Parent = null;
        return rst;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }
}
