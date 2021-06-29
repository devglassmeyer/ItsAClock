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

        internal static class mySettings
        {
            private static object _lockMe = new object();
            private static bool _showSeconds = true;
            private static int _lastMinute = 0;
            
            internal static int LastMinute
            {
                get
                {
                    int ret = 0;
                    lock (_lockMe) { ret = _lastMinute; }
                    return ret;
                }
                set
                {
                    lock (_lockMe) { _lastMinute = value; }
                }
            }

            internal static bool ShowSeconds
            {
                get
                {
                    bool ret = true;
                    lock (_lockMe) { ret = _showSeconds; }
                    return ret;
                }
                set
                {
                    lock (_lockMe) 
                    { 
                        _showSeconds = value; 
                        if (!_showSeconds) { _lastMinute = 0; }
                    }
                }
            }
        }


        private bool _mouseIsDown = false;
        private Point _lastLocation;
        private additionalTZStruct[] _addedTimeZones = null;
        private object _lockMe = new object();
        private List<string> _selectedTimeZoneIDs = new List<string>();
        private int _startingHeight = 0;
        private int _startingWidth = 0;
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
                    _addedTimeZones[i].dateLabel.MouseDown -= new MouseEventHandler(lbl_added_MouseDown);
                    _addedTimeZones[i].dateLabel.MouseMove -= new MouseEventHandler(lbl_added_MouseMove);
                    _addedTimeZones[i].dateLabel.MouseUp -= new MouseEventHandler(lbl_added_MouseUp);

                    _addedTimeZones[i].timeLabel.MouseDown -= new MouseEventHandler(lbl_added_MouseDown);
                    _addedTimeZones[i].timeLabel.MouseMove -= new MouseEventHandler(lbl_added_MouseMove);
                    _addedTimeZones[i].timeLabel.MouseUp -= new MouseEventHandler(lbl_added_MouseUp);

                    _addedTimeZones[i].timezonelabel.MouseDown -= new MouseEventHandler(lbl_added_MouseDown);
                    _addedTimeZones[i].timezonelabel.MouseMove -= new MouseEventHandler(lbl_added_MouseMove);
                    _addedTimeZones[i].timezonelabel.MouseUp -= new MouseEventHandler(lbl_added_MouseUp);

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
                mySettings.LastMinute = 0;
                _addedTimeZones = new additionalTZStruct[_selectedTimeZoneIDs.Count];
                if (mySettings.ShowSeconds)
                {
                    this.Size = new Size(_startingWidth, _startingHeight + (((_clientHeight - (lblDate.Top / 2)) / 2) * _selectedTimeZoneIDs.Count));
                }
                else
                {
                    this.Size = new Size(_startingWidth - 30, _startingHeight + (((_clientHeight - (lblDate.Top / 2)) / 2) * _selectedTimeZoneIDs.Count));
                }

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

                    _addedTimeZones[i].dateLabel.MouseDown += new MouseEventHandler(lbl_added_MouseDown);
                    _addedTimeZones[i].dateLabel.MouseMove += new MouseEventHandler(lbl_added_MouseMove);
                    _addedTimeZones[i].dateLabel.MouseUp += new MouseEventHandler(lbl_added_MouseUp);

                    _addedTimeZones[i].timeLabel.MouseDown += new MouseEventHandler(lbl_added_MouseDown);
                    _addedTimeZones[i].timeLabel.MouseMove += new MouseEventHandler(lbl_added_MouseMove);
                    _addedTimeZones[i].timeLabel.MouseUp += new MouseEventHandler(lbl_added_MouseUp);

                    _addedTimeZones[i].timezonelabel.MouseDown += new MouseEventHandler(lbl_added_MouseDown);
                    _addedTimeZones[i].timezonelabel.MouseMove += new MouseEventHandler(lbl_added_MouseMove);
                    _addedTimeZones[i].timezonelabel.MouseUp += new MouseEventHandler(lbl_added_MouseUp);
                }
            }
            else
            {
                this.Height = _startingHeight;
                if (mySettings.ShowSeconds)
                {
                    this.Width = _startingWidth;
                }
                else
                {
                    this.Width = _startingWidth - 30;
                }
                
            }
            IsPainInProgress = false;
        }

        private void PaintSecondsChange()
        {
            IsPainInProgress = true;
            const int TIME_SECONDS_WIDTH = 150;
            const int TIME_NOSECONDS_WIDTH = 115;
            const int TIMEZONE_SECONDS_LEFT = 260;
            const int TIMEZONE_NOSECONDS_LEFT = TIMEZONE_SECONDS_LEFT - (TIME_SECONDS_WIDTH - TIME_NOSECONDS_WIDTH);
            if (mySettings.ShowSeconds)
            {
                lblMainTime.Width = TIME_SECONDS_WIDTH;
                lblTimeZone.Left = TIMEZONE_SECONDS_LEFT;
            }
            else
            {
                lblMainTime.Width = TIME_NOSECONDS_WIDTH;
                lblTimeZone.Left = TIMEZONE_NOSECONDS_LEFT;
            }
            PaintNewTimezones();
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
                int last_minute = mySettings.LastMinute;
                bool show_seconds = mySettings.ShowSeconds;
                if ((last_minute == currentTime.Minute) && !mySettings.ShowSeconds && (last_minute != 0))
                {
                    return;
                }

                string time_format = show_seconds ? "h:mm:ss tt" : "h:mm tt";
                mySettings.LastMinute = currentTime.Minute;

                string time_string = currentTime.ToString(time_format);
                string date_string = currentTime.ToString("M-dd-yy");
                lblTimeZone.Text = TimeZoneInfo.Local.DisplayName;

                lblDate.Text = date_string;
                lblMainTime.Text = time_string;

                if (_addedTimeZones != null)
                {
                    for (int i = 0; i < _addedTimeZones.Length; i++)
                    {
                        DateTime convertedTime = TimeZoneInfo.ConvertTime(currentTime, _addedTimeZones[i].tzInfo);
                        _addedTimeZones[i].dateLabel.Text = convertedTime.ToString("M-dd-yy");
                        _addedTimeZones[i].timeLabel.Text = convertedTime.ToString(time_format);
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
            _startingWidth = this.Width;
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbl_added_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseIsDown = true;
            _lastLocation = e.Location;
        }
        private void lbl_added_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseIsDown)
            {
                this.Location = new Point(
                    (this.Location.X - _lastLocation.X) + e.X, (this.Location.Y - _lastLocation.Y) + e.Y);

                this.Update();
            }
        }
        private void lbl_added_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseIsDown = false;
        }

        private void toolStropShowSeconds_CheckStateChanged(object sender, EventArgs e)
        {
            mySettings.ShowSeconds = toolStropShowSeconds.Checked;
            PaintSecondsChange();
        }
    }
}
