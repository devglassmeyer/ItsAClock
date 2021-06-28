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
    public partial class frmTimeZones : Form
    {
        struct myTZ     // why a struct and not a class? I don't know, I guess I felt like it
        {
            public string TimezoneID { get; set; }
            public bool IsSelected { get; set; }
            public myTZ(string time_zone_id, bool is_selected)
            {
                TimezoneID = time_zone_id;
                IsSelected = is_selected;
            }
        }
        private bool _okIsClicked = false;
        private myTZ[] _originalSelection = new myTZ[0];
        private bool _processingCheck = false;

        public bool IsOkClicked
        {
            get { return _okIsClicked; }
        }

        /// <summary>
        /// Get a list of TimeZone ID string values selected by the user
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedTimeZones()
        {
            List<string> ret = new List<string>();

            for (int i = 0; i < chkTimeZone.Items.Count; i++)
            {
                // only return non-local time zone ID's
                if (chkTimeZone.GetItemChecked(i) && (_originalSelection[i].TimezoneID != TimeZoneInfo.Local.Id))
                {
                    ret.Add(_originalSelection[i].TimezoneID);
                }
            }

            return ret;
        }

        public frmTimeZones(List<string> timezoneIDList)
        {
            InitializeComponent();

            lblMain.Text = "Select all the time zone's to show on the main clock. Press OK to save or Cancel to not save.";
            LoadTimeZones(timezoneIDList);
            PaintOK();
        }

        private void LoadTimeZones(List<string> currentTimeZones)
        {
            _originalSelection = new myTZ[TimeZoneInfo.GetSystemTimeZones().Count];
            for (int i = 0; i < _originalSelection.Length; i++)
            {
                _originalSelection[i] = new myTZ("", false);
            }
            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                if ((tz.Id == TimeZoneInfo.Local.Id) || (currentTimeZones.Contains(tz.Id)))
                {
                    // this timezone is selected by the user or is local
                    int myindex = this.chkTimeZone.Items.Add(tz.DisplayName, true);
                    _originalSelection[myindex].TimezoneID = tz.Id;
                    _originalSelection[myindex].IsSelected = true;
                }
                else
                {
                    int myindex = this.chkTimeZone.Items.Add(tz.DisplayName, false);
                    _originalSelection[myindex].TimezoneID = tz.Id;
                    _originalSelection[myindex].IsSelected = false;
                }
            }
        }

        private void PaintOK()
        {
            if (IsChanged())
            {
                this.btnOK.Enabled = true;
            }
            else
            {
                this.btnOK.Enabled = false;
            }
        }

        /// <summary>
        /// Return true if the selected list of time zones has changed
        /// </summary>
        /// <returns></returns>
        private bool IsChanged()
        {
            for (int i = 0; i < this.chkTimeZone.Items.Count; i++)
            {
                if (chkTimeZone.GetItemChecked(i) != _originalSelection[i].IsSelected)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _okIsClicked = true;
            this.Hide();
        }

        private void chkTimeZone_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_processingCheck) { return; }
            _processingCheck = true;

            CheckedListBox clb = (CheckedListBox)sender;
            // Switch off event handler
            // make sure we do not un-check our current timezone
            if (_originalSelection[e.Index].TimezoneID == TimeZoneInfo.Local.Id)
            {
                // this is our current time zone -> yell at the user
                MessageBox.Show(this, "You cannot disable your current time zone, after all how would you know what time it is?", "No You Don't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.NewValue = CheckState.Checked;
            }
            else
            {
                clb.SetItemCheckState(e.Index, e.NewValue);
                PaintOK();
            }
            _processingCheck = false;
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkTimeZone.Items.Count; i++)
            {
                if (_originalSelection[i].TimezoneID != TimeZoneInfo.Local.Id)
                {
                    chkTimeZone.SetItemChecked(i, false);
                }
            }
        }
    }
}
