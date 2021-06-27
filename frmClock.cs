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
    public partial class frmClock : Form
    {
        delegate void noparms();

        private void PaintTime()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new noparms(PaintTime));
                    return;
                }
                DateTime currentTime = DateTime.Now;

                string time_string = currentTime.ToString("h:mm:ss");
                string date_string = currentTime.ToString("M-dd-yyyy");

                lblPM.Text = (currentTime.Hour > 12) ? "PM" : "AM";
                lblDate.Text = date_string;
                lblMainTime.Text = time_string;
            }
            catch { }   // ignore any exceptions
        }
        
        /// <summary>
        /// Create background thread to update the time using Threadpool
        /// </summary>
        private void StartClockThread()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    PaintTime();
                    // lets wait here a bit for some time to go by
                    System.Threading.Thread.Sleep(250);
                }
            });
        }
        
        public frmClock()
        {
            InitializeComponent();
            StartClockThread();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout aboutMe = new frmAbout();

            aboutMe.ShowDialog(this);
        }
    }
}
