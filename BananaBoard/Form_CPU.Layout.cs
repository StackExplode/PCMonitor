using BananaBoard.Controls;
using BananaBoard.Hardware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BananaBoard;
internal partial class Form_CPU
{
    private LCDLabel lbl_cpu_usasge, lbl_cpu_temp, lbl_cpu_voltage, lbl_cpu_freqp, lbl_cpu_freqe, lbl_cpu_pow;

    private void InitUIControls()
    {
        this.Position = new Point(0, 0);
        this.Size = new Size(140, 100);
        this.BackColor = Color.Black;

        LCDPicturebox pic = new LCDPicturebox(Device);
        pic.Image = Image.FromFile("cpu.png");
        pic.StretchType = ImageStretchType.Fit;
        pic.Position = new Point(0, 0);
        pic.BackColor = this.BackColor;
        Controls.Add(pic);

        lbl_cpu_usasge = new LCDLabel(Device);
        lbl_cpu_usasge.Position = new Point(64, 0);
        lbl_cpu_usasge.BackColor = this.BackColor;
        lbl_cpu_usasge.ForeColor = Color.Red;
        lbl_cpu_usasge.Font = new Font("JLCDFont2", 26);
        lbl_cpu_usasge.Text = "100%";
        Controls.Add(lbl_cpu_usasge);

        lbl_cpu_temp = new LCDLabel(Device);
        lbl_cpu_temp.Position = new Point(64, 30);
        lbl_cpu_temp.BackColor = this.BackColor;
        lbl_cpu_temp.Font = new Font("JLCDFont2", 26);
        lbl_cpu_temp.Text = "88&";
        lbl_cpu_temp.ForeColor = Color.Yellow;
        Controls.Add(lbl_cpu_temp);

        lbl_cpu_voltage = new LCDLabel(Device);
        lbl_cpu_voltage.Position = new Point(70, 82);
        lbl_cpu_voltage.BackColor = this.BackColor;
        lbl_cpu_voltage.Font = new Font("SimHei", 12);
        lbl_cpu_voltage.Text = "1.88 V";
        lbl_cpu_voltage.ForeColor = Color.OrangeRed;
        Controls.Add(lbl_cpu_voltage);

        lbl_cpu_freqp = new LCDLabel(Device);
        lbl_cpu_freqp.Position = new Point(0, 64);
        lbl_cpu_freqp.BackColor = this.BackColor;
        lbl_cpu_freqp.Font = new Font("DejaVu Sans", 12);
        lbl_cpu_freqp.Text = "5.1 GHz";
        lbl_cpu_freqp.ForeColor = Color.FromArgb(0xf0, 0x00, 0x56);
        Controls.Add(lbl_cpu_freqp);

        lbl_cpu_freqe = new LCDLabel(Device);
        lbl_cpu_freqe.Position = new Point(0, 80);
        lbl_cpu_freqe.BackColor = this.BackColor;
        lbl_cpu_freqe.Font = new Font("DejaVu Sans", 12);
        lbl_cpu_freqe.Text = "3.6 GHz";
        lbl_cpu_freqe.ForeColor = Color.FromArgb(0x00, 0xbf, 0xff);
        Controls.Add(lbl_cpu_freqe);

        lbl_cpu_pow = new LCDLabel(Device);
        lbl_cpu_pow.Position = new Point(70, 64);
        lbl_cpu_pow.BackColor = this.BackColor;
        lbl_cpu_pow.Font = new Font("DejaVu Sans", 12);
        lbl_cpu_pow.Text = "200 W";
        lbl_cpu_pow.ForeColor = Color.Aqua;
        Controls.Add(lbl_cpu_pow);

    }
}
