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

        struct additionalTZStruct
        {
            public Label dateLabel;
            public Label timeLabel;
            public Label timezonelabel;
            public TimeZoneInfo tzInfo;
        }

        private bool _mouseIsDown = false;
        private Point _lastLocation;
        private additionalTZStruct[] _addedTimeZones = null;
        private object _lockMe = new object();
        private List<string> _selectedTimeZoneIDs = new List<string>();
        private int _startingHeight = 0;
        private int _clientHeight = 0;
        private bool _isRePaintInProgress = false;

        private bool IsPainInProgress
        {
            get
            {
                bool ret = false;
                lock (_lockMe) { ret = _isRePaintInProgress; }
                return ret;
            }
            set
            {
                lock (_lockMe) { _isRePaintInProgress = value; }
            }
        }

        private void CleanUp()
        {
            if (_addedTimeZones != null)
            {
                for (int i =0; i < _addedTimeZones.Length; i++)
                {
                    this.Controls.Remove(_addedTimeZones[i].dateLabel);
                    this.Controls.Remove(_addedTimeZones[i].timeLabel);
                    this.Controls.Remove(_addedTimeZones[i].timezonelabel);
                }
                _addedTimeZones = null;
            }
        }

        private Label CreateLabelOffAnother(Label basedOffLabel)
        {
            Label newLabel = new Label();
            this.Controls.Add(newLabel);
            newLabel.AutoSize = basedOffLabel.AutoSize;
            newLabel.Font = basedOffLabel.Font;
            newLabel.Width = basedOffLabel.Width;
            newLabel.Height = basedOffLabel.Height;
            newLabel.BackColor = basedOffLabel.BackColor;
            newLabel.ForeColor = basedOffLabel.ForeColor;
            newLabel.TextAlign = basedOffLabel.TextAlign;
            return newLabel;
        }

        private void PaintNewTimezones()
        {
            IsPainInProgress = true;

            CleanUp();

            if (_selectedTimeZoneIDs.Count > 0)
            {
                _addedTimeZones = new additionalTZStruct[_selectedTimeZoneIDs.Count];
                this.Size = new Size(this.Width, _startingHeight + (((_clientHeight - (lblDate.Top / 2)) / 2) * _selectedTimeZoneIDs.Count));

                for (int i = 0; i < _selectedTimeZoneIDs.Count; i++)
                {
                    _addedTimeZones[i].dateLabel = CreateLabelOffAnother(this.lblDate);
                    _addedTimeZones[i].timeLabel = CreateLabelOffAnother(lblMainTime);
                    _addedTimeZones[i].timezonelabel = CreateLabelOffAnother(this.lblTimeZone);
                    _addedTimeZones[i].tzInfo = TimeZoneInfo.FindSystemTimeZoneById(_selectedTimeZoneIDs[i]);


                    // position controls
                    int topBuffer = 5;
                    _addedTimeZones[i].dateLabel.Top = lblDate.Top + (topBuffer * (i + 1)) + (lblDate.Height * (i + 1));
                    _addedTimeZones[i].dateLabel.Left = lblDate.Left;

                    _addedTimeZones[i].timeLabel.Top = lblMainTime.Top + (topBuffer * (i + 1)) + (lblMainTime.Height * (i + 1));
                    _addedTimeZones[i].timeLabel.Left = lblMainTime.Left;

                    _addedTimeZones[i].timezonelabel.Top = lblTimeZone.Top + (topBuffer * (i + 1)) + (lblTimeZone.Height * (i + 1));
                    _addedTimeZones[i].timezonelabel.Left = lblTimeZone.Left;
                    _addedTimeZones[i].timezonelabel.Text = _addedTimeZones[i].tzInfo.DisplayName;

                }
            }
            else
            {
                this.Height = _startingHeight;
            }
            IsPainInProgress = false;
        }

        private void PaintTime()
        {
            if (IsPainInProgress) { return; }
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new noparms(PaintTime));
                    return;
                }
                DateTime currentTime = DateTime.Now;

                string time_string = currentTime.ToString("h:mm:ss tt");
                string date_string = currentTime.ToString("M-dd-yyyy");
                lblTimeZone.Text = TimeZone.CurrentTimeZone.IsDaylightSavingTime(currentTime) ? TimeZone.CurrentTimeZone.DaylightName : TimeZone.CurrentTimeZone.StandardName;
                lblDate.Text = date_string;
                lblMainTime.Text = time_string;

                if (_addedTimeZones != null)
                {
                    for (int i = 0; i < _addedTimeZones.Length; i++)
                    {
                        DateTime convertedTime = TimeZoneInfo.ConvertTime(currentTime, _addedTimeZones[i].tzInfo);
                        _addedTimeZones[i].dateLabel.Text = convertedTime.ToString("M-dd-yyyy");
                        _addedTimeZones[i].timeLabel.Text = convertedTime.ToString("h:mm:ss tt");
                    }
                }

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
                    if (!IsPainInProgress)
                    {
                        PaintTime();
                    }
                    // lets wait here a bit for some time to go by
                    System.Threading.Thread.Sleep(250);
                }
            });
        }
        
        public frmClock()
        {
            InitializeComponent();
            StartClockThread();
            _startingHeight = this.Height;
            _clientHeight = this.ClientSize.Height;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout aboutMe = new frmAbout();

            aboutMe.ShowDialog(this);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmTimeZones tzForm = new frmTimeZones(_selectedTimeZoneIDs);
            tzForm.ShowDialog(this);

            if (tzForm.IsOkClicked)
            {
                _selectedTimeZoneIDs = tzForm.GetSelectedTimeZones();
                PaintNewTimezones();
            }
        }

        private void frmClock_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseIsDown = true;
            _lastLocation = e.Location;
        }

        private void frmClock_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseIsDown)
            {
                this.Location = new Point(
                    (this.Location.X - _lastLocation.X) + e.X, (this.Location.Y - _lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void frmClock_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseIsDown = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
