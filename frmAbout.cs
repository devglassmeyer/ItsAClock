using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

            lblMain.Text = "Hey, this isn't that complicated. It is a clock.";
            lblMain.Text += Environment.NewLine;
            lblMain.Text += Environment.NewLine;
            lblMain.Text += "Select your favorite time zones to show the current date and time. Be amazed as the time ticks by one second at a time.";
            lblMain.Text += Environment.NewLine;
            lblMain.Text += Environment.NewLine;

            var v = Assembly.GetExecutingAssembly().GetName().Version;

            lblMain.Text += "Version " + " " + v.Major + "." + v.Minor + " (build " + v.Build + ")"; 
        }
    }
}
