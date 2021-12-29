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
    public partial class frmTimeZonePicker : Form
    {
        private const int _COLUMN_TIMEZONE_NAME = 0;
        private const int _COLUMN_CUSTOM_NAME = 1;
        private const int _COLUMN_SHOW_TIME = 2;
        private const int _COLUMN_INCLUDE_IN_LIST = 3;
        private const int _COLUMN_TIME_ZONE_ID = 4;
        private const int _COLUMN_SHOW_USER_FRIENDLY_NAME = 5;

        private bool _okIsClicked = false;
        private TZDataList _tz_data = new TZDataList();
        private TZDataList _current_TZ = new TZDataList();

        public frmTimeZonePicker(TZDataList current_list)
        {
            _current_TZ = new TZDataList(current_list);
            LoadTimeZoneList();
            InitializeComponent();
            lblMain.Text = "Select which time zones to include in your list. Select which time zones to show the time. You can even use a custom name when for a time zone. How cool is that eh?";
            SetupGrid();
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

        private void SetupGrid()
        {
            dgTimeZoneData.DataSource = _tz_data.TimeZoneData;
            dgTimeZoneData.Columns[_COLUMN_TIMEZONE_NAME].HeaderText = "Timezone Name";
            dgTimeZoneData.Columns[_COLUMN_TIMEZONE_NAME].Width = 325;

            dgTimeZoneData.Columns[_COLUMN_CUSTOM_NAME].HeaderText = "Custom Name";
            dgTimeZoneData.Columns[_COLUMN_CUSTOM_NAME].Width = 250;

            dgTimeZoneData.Columns[_COLUMN_SHOW_TIME].HeaderText = "Show Time";
            dgTimeZoneData.Columns[_COLUMN_SHOW_TIME].Width = 45;

            dgTimeZoneData.Columns[_COLUMN_INCLUDE_IN_LIST].HeaderText = "Include";
            dgTimeZoneData.Columns[_COLUMN_INCLUDE_IN_LIST].Width = 45;

            dgTimeZoneData.Columns[_COLUMN_TIME_ZONE_ID].HeaderText = "TZID";
            dgTimeZoneData.Columns[_COLUMN_TIME_ZONE_ID].Visible = false;

            dgTimeZoneData.Columns[_COLUMN_SHOW_USER_FRIENDLY_NAME].HeaderText = "Show Custom Name";
            dgTimeZoneData.Columns[_COLUMN_SHOW_USER_FRIENDLY_NAME].Width = 45;
        }

        private void LoadTimeZoneList()
        {
            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                _tz_data.Add_TimeZone(tz.Id, tz.DisplayName, (tz.Id == TimeZoneInfo.Local.Id));
            }

            _tz_data.SetCurrentData(_current_TZ);
        }

        public bool IsOkClicked
        {
            get { return _okIsClicked; }
        }

        public TZDataList GetIncludedTZList()
        {
            return _tz_data.GetIncludedData();
        }

        private void dgTimeZoneData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgTimeZoneData_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void dgTimeZoneData_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.RowIndex > -1) && ((e.ColumnIndex == _COLUMN_SHOW_TIME) || (e.ColumnIndex == _COLUMN_INCLUDE_IN_LIST)))
            {
                var tz = dgTimeZoneData.Rows[e.RowIndex].DataBoundItem as TZData;

                if (tz.IsLocalTimezone())
                {
                    MessageBox.Show(this, "You cannot disable your current time zone, after all how would you know what time it is?", "No You Don't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgTimeZoneData.CurrentCell = dgTimeZoneData.Rows[e.RowIndex].Cells[_COLUMN_CUSTOM_NAME];
                    tz.ShowTimeForTimeZone = true;
                    tz.IncludeInList = true;
                }
            }
        }
    }
}
