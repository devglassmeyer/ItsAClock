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
            private static string _saveFileName = "";
            
            internal static string SaveFileName
            {
                get 
                { 
                    if (string.IsNullOrWhiteSpace(_saveFileName))
                    {
                        _saveFileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ItsAClockSettings.xml");
                    }
                    return _saveFileName; 
                }
                set { _saveFileName = value; }
            }
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

        private int _rightclick_index = -1;
        private bool _mouseIsDown = false;
        private Point _lastLocation;
        private additionalTZStruct[] _addedTimeZones = null;
        private object _lockMe = new object();
        private TZDataList _selected_TZ = new TZDataList();
        private int _startingHeight = 0;
        private int _startingWidth = 0;
        private int _clientHeight = 0;
        private bool _isRePaintInProgress = false;
        private bool _isLoading = false;


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

        private void PaintNewTimezones2()
        {
            IsPainInProgress = true;

            CleanUp();

            var timezones_to_show = _selected_TZ.TimeZoneData.FindAll(x => x.ShowTimeForTimeZone == true);

            if ((timezones_to_show!=null) && (timezones_to_show.Count > 0))
            {
                mySettings.LastMinute = 0;
                _addedTimeZones = new additionalTZStruct[timezones_to_show.Count];
                if (mySettings.ShowSeconds)
                {
                    this.Size = new Size(_startingWidth, _startingHeight + (((_clientHeight - (lblDate.Top / 2)) / 2) * timezones_to_show.Count));
                }
                else
                {
                    this.Size = new Size(_startingWidth - 30, _startingHeight + (((_clientHeight - (lblDate.Top / 2)) / 2) * timezones_to_show.Count));
                }
                for (int i = 0; i < timezones_to_show.Count; i++)
                {
                    _addedTimeZones[i].dateLabel = CreateLabelOffAnother(this.lblDate);
                    _addedTimeZones[i].dateLabel.Tag = i;
                    _addedTimeZones[i].timeLabel = CreateLabelOffAnother(lblMainTime);
                    _addedTimeZones[i].timeLabel.Tag = i;
                    _addedTimeZones[i].timezonelabel = CreateLabelOffAnother(this.lblTimeZone);
                    _addedTimeZones[i].timezonelabel.Tag = i;
                    _addedTimeZones[i].tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timezones_to_show[i].TimeZoneID);

                    // position controls
                    int topBuffer = 5;
                    _addedTimeZones[i].dateLabel.Top = lblDate.Top + (topBuffer * (i + 1)) + (lblDate.Height * (i + 1));
                    _addedTimeZones[i].dateLabel.Left = lblDate.Left;

                    _addedTimeZones[i].timeLabel.Top = lblMainTime.Top + (topBuffer * (i + 1)) + (lblMainTime.Height * (i + 1));
                    _addedTimeZones[i].timeLabel.Left = lblMainTime.Left;

                    _addedTimeZones[i].timezonelabel.Top = lblTimeZone.Top + (topBuffer * (i + 1)) + (lblTimeZone.Height * (i + 1));
                    _addedTimeZones[i].timezonelabel.Left = lblTimeZone.Left;
                    _addedTimeZones[i].timezonelabel.Text = timezones_to_show[i].IsCustomNameOn() ? timezones_to_show[i].CustomName : _addedTimeZones[i].tzInfo.DisplayName;

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
            PaintNewTimezones2();
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
                if (mySettings.ShowSeconds != _selected_TZ.IsSecondsOn)
                {
                    // this will happen on startup 
                    // we do this here to make sure all labels are loaded and created OK
                    mySettings.ShowSeconds = _selected_TZ.IsSecondsOn;
                    _isLoading = true;
                    toolStropShowSeconds.Checked = mySettings.ShowSeconds;
                    _isLoading = false;
                    PaintSecondsChange();
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
                
                lblTimeZone.Text = _selected_TZ.IsCustomNameOn() ? _selected_TZ.CustomName : TimeZoneInfo.Local.DisplayName;

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
            _isLoading = true;
            InitializeComponent();
            StartClockThread();
            _startingHeight = this.Height;
            _startingWidth = this.Width;
            _clientHeight = this.ClientSize.Height;

            if (System.IO.File.Exists(mySettings.SaveFileName))
            {
                try
                {
                    _selected_TZ = FileUtil.ReadFromXmlFile<TZDataList>(mySettings.SaveFileName);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("Error reading " + mySettings.SaveFileName + Environment.NewLine + ex.ToString());
                }
            }
            if ((_selected_TZ!=null) && (_selected_TZ.TimeZoneData.Count > 0))
            {
                PaintNewTimezones2();
            }
            _isLoading = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout aboutMe = new frmAbout();

            aboutMe.ShowDialog(this);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmTimeZonePicker tzPick = new frmTimeZonePicker(_selected_TZ);
            tzPick.ShowDialog(this);
            if (tzPick.IsOkClicked)
            {
                _selected_TZ = tzPick.GetIncludedTZList();
                try
                {
                    FileUtil.WriteToXmlFile<TZDataList>(mySettings.SaveFileName, _selected_TZ);
                }
                catch (Exception ex)
                {
                    // output error here
                    System.Diagnostics.Trace.WriteLine("Error saving time zone data: " + Environment.NewLine + ex.ToString());
                }
                PaintNewTimezones2();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbl_added_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseIsDown = true;
                _lastLocation = e.Location;
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    // show menu of selections
                    Label you_clicked_on_me = sender as Label;

                    if (you_clicked_on_me == null)
                    {
                        // we clicked on the form - not one of the labels - do nothing here
                        return;
                    }

                    string custom_name = string.Empty;
                    string display_name = string.Empty;
                    bool is_customname_in_use = false;
                    bool has_custom_name = false;

                    if (you_clicked_on_me.Tag == null)
                    {
                        has_custom_name = _selected_TZ.HasCustomName();
                        if (has_custom_name)
                        {
                            _rightclick_index = -1; // our top main time zone - local to us, the center of our bubble
                            is_customname_in_use = _selected_TZ.IsCustomNameOn();
                            custom_name = _selected_TZ.CustomName;
                            display_name = TimeZoneInfo.Local.DisplayName;
                        }
                    }
                    else
                    {
                        int my_index = (int)you_clicked_on_me.Tag;
                        string timezone_id = _addedTimeZones[my_index].tzInfo.Id;
                        var myTZ = _selected_TZ.TimeZoneData.FirstOrDefault(x => x.TimeZoneID == timezone_id);
                        has_custom_name = myTZ.HasCustomName();
                        if (has_custom_name)
                        {
                            _rightclick_index = my_index;
                            custom_name = myTZ.CustomName;
                            display_name = _addedTimeZones[my_index].tzInfo.DisplayName;
                            is_customname_in_use = myTZ.UseCustomName;
                        }
                    }

                    if (has_custom_name)
                    {
                        ContextMenu cm = new ContextMenu();
                        MenuItem mu = new MenuItem(custom_name);
                        if (is_customname_in_use)
                        {
                            mu.Checked = true;
                        }
                        else
                        {
                            mu.Checked = false;
                            mu.Click += Mu_Click;
                        }
                        MenuItem mu_displayname = new MenuItem(display_name);
                        mu_displayname.Checked = !mu.Checked;
                        if (!mu_displayname.Checked)
                        {
                            mu_displayname.Click += Mu_Click;
                        }
                        cm.MenuItems.Add(mu);
                        cm.MenuItems.Add(mu_displayname);

                        cm.Show(this, new Point(e.X + ((Control)sender).Left + 20, e.Y + ((Control)sender).Top + 20));
                    }
                }
            }
        }

        private void Mu_Click(object sender, EventArgs e)
        {
            if (_rightclick_index == -1)
            {
                // main - local time zone
                _selected_TZ.UseCustomName = !_selected_TZ.UseCustomName;
                lblTimeZone.Text = _selected_TZ.UseCustomName ? _selected_TZ.CustomName : TimeZoneInfo.Local.DisplayName;
            }
            else
            {
                string timezone_id = _addedTimeZones[_rightclick_index].tzInfo.Id;
                var myTZ = _selected_TZ.TimeZoneData.FirstOrDefault(x => x.TimeZoneID == timezone_id);
                myTZ.UseCustomName = !myTZ.UseCustomName;

                _addedTimeZones[_rightclick_index].timezonelabel.Text = myTZ.UseCustomName ? myTZ.CustomName : _addedTimeZones[_rightclick_index].tzInfo.DisplayName;
            }
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
            if (!_isLoading)
            {
                mySettings.ShowSeconds = toolStropShowSeconds.Checked;
                _selected_TZ.IsSecondsOn = mySettings.ShowSeconds;
                try { FileUtil.WriteToXmlFile<TZDataList>(mySettings.SaveFileName, _selected_TZ); }
                catch (Exception ex) { System.Diagnostics.Trace.WriteLine("Error saving " + mySettings.SaveFileName + Environment.NewLine + ex.ToString()); }
                PaintSecondsChange();
            }
        }

        private void saveTimezonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSaveInfo s = new frmSaveInfo(mySettings.SaveFileName, _selected_TZ);
            s.ShowDialog(this);

            if (s.IsFileLoaded())
            {
                // we have a new file and new data!
                _selected_TZ = s.GetTZData();
                mySettings.SaveFileName = s.SaveFileName();
                try { FileUtil.WriteToXmlFile<TZDataList>(mySettings.SaveFileName, _selected_TZ); }
                catch (Exception ex)
                {
                    // output error here
                    System.Diagnostics.Trace.WriteLine("Error saving time zone data: " + Environment.NewLine + ex.ToString());
                }
                PaintNewTimezones2();
                return;
            }

            string test_file = s.SaveFileName();
            if (!string.IsNullOrWhiteSpace(test_file) && (test_file != mySettings.SaveFileName))
            {
                try
                {
                    if (!System.IO.File.Exists(test_file))
                    {
                        // try to create the file
                        try
                        {
                            FileUtil.WriteToXmlFile<TZDataList>(test_file, _selected_TZ);
                        }
                        catch (Exception ex) { System.Diagnostics.Trace.WriteLine("Error tring to save test file " + test_file + Environment.NewLine + ex.ToString()); }

                        if (System.IO.File.Exists(test_file))
                        {
                            // this should be true - if not we do not want to use this file name
                            mySettings.SaveFileName = test_file;
                        }
                    }

                    if (System.IO.File.Exists(test_file))
                    {
                        mySettings.SaveFileName = test_file;

                    }
                }
                catch (Exception ex) { System.Diagnostics.Trace.WriteLine("Error checking if file exists " + test_file + Environment.NewLine + ex.ToString()); }

            }

        }
    }
}
