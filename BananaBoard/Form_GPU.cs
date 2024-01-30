using BananaBoard.Controls;
using BananaBoard.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BananaBoard;
internal partial class Form_GPU : LCDForm
{
    public Form_GPU(SPILCD dev) : base(dev)
    {
        InitUILayout();
    }
}
