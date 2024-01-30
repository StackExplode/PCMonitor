using BananaBoard.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BananaBoard;
internal partial class Form_GPU
{
    LCDPicturebox pic1;
    LCDLabel lbl_uage, lbl_temp;
    void InitUILayout()
    {
        this.Size = new Size(320 - 140, 100);
        this.Position = new Point(140, 0);
        this.BackColor = Color.Black;

        pic1 = new LCDPicturebox(Device);
        pic1.Position = new Point(0, 0);
        pic1.BackColor = this.BackColor;
        pic1.StretchType = ImageStretchType.Fit;
        pic1.Image = Bitmap.FromFile("GPU.png");
        this.Controls.Add(pic1);

        lbl_uage = new LCDLabel(Device);
        lbl_uage.Position = new Point(64, 0);
        lbl_uage.BackColor = this.BackColor;
        lbl_uage.ForeColor = Color.Red;
        lbl_uage.Font = new Font("JLCDFont2", 26);
        lbl_uage.Text = "100%";
        Controls.Add(lbl_uage);

        lbl_temp = new LCDLabel(Device);
        lbl_temp.Position = new Point(64, 30);
        lbl_temp.BackColor = this.BackColor;
        lbl_temp.Font = new Font("JLCDFont2", 26);
        lbl_temp.Text = "88&";
        lbl_temp.ForeColor = Color.Yellow;
        Controls.Add(lbl_temp);
    }
}
