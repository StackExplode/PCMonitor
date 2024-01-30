using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCMonitorCommon;
using PCMonitorCommon.Driver;
using PCMonitorCommon.Entity.Obsolete;
using PCMonitorCommon.Test;

namespace PCMonitorTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        struct Temp
        {
            public byte a;
            public byte b; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[] {
                2,0,0,0,
                1,0,
                4,
                5,
                (byte)'a',7,0,
                1,0,0,0,
                2, 0, 0, 0,
                3, 0, 0, 0, };

            BufferReader br = new BufferReader(data);
            var a = br.Read<int>();
            var b = br.Read<ushort>();
            var c = br.Read<byte>();
            br.Seek(+1);
            var d = br.ReadKeyValuePair<sbyte,short>();
            br.Seek(-3);
            var d2 = new Dictionary<sbyte, short>();
            br.ReadDictionary(d2, 1);
            int[] arr = br.ReadArray<int>(3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TestTasks.Run();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WoLDriverWindows wol = new WoLDriverWindows(34);
            wol.SendWoL("00:90:9e:9a:a1:0c");
                     
        }
    }
}
