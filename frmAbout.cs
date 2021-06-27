using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItsAClock
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();

            lblMain.Text = "Hey, this isn't complicated. This is a clock.";
            lblMain.Text += Environment.NewLine;
            lblMain.Text += Environment.NewLine;
            lblMain.Text += "Press that X in the upper right to close.";
        }
    }
}
