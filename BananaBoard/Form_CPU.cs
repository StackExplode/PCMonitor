using BananaBoard.Controls;
using BananaBoard.Hardware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using TTimer = System.Timers.Timer;
namespace BananaBoard;
internal partial class Form_CPU : LCDForm
{

    public Form_CPU(SPILCD dev) : base(dev)
    {
        InitUIControls();
    }

    
    public void TestChangeUI()
    {
        Invoke(() => { lbl_cpu_temp.Text = "我是汉字"; });
        Invoke(() => { lbl_cpu_usasge.Text = "CCCCC"; });
    }

    
}
