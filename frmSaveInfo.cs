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
    public partial class frmSaveInfo : Form
    {
        private string _save_filename = string.Empty;
        private bool _is_ok_pressed = false;
        private bool _is_file_loaded = false;
        private TZDataList _tzdata = new TZDataList();
        public frmSaveInfo(string save_filename, TZDataList tzdata)
        {
            InitializeComponent();
            _tzdata = tzdata;
            _save_filename = save_filename;
            lblMain.Text = "Save your time zone’s. We try to automatically save the selected time zones into the file below. You can change the file name and click the save or load button to use a different file. Give it a try.";
            lblFileExists.Text = "";
            txtSaveFilename.Text = save_filename;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _is_ok_pressed = true;
            this.Hide();
        }

        private void txtSaveFilename_TextChanged(object sender, EventArgs e)
        {
            SetFileExist(Does_file_exist());
        }

        public bool IsOkPressed()
        {
            return _is_ok_pressed;
        }
        public bool IsFileLoaded()
        {
            return _is_file_loaded;
        }
        public string SaveFileName()
        {
            return txtSaveFilename.Text;
        }
        public TZDataList GetTZData()
        {
            return _tzdata;
        }
        private void SetFileExist(bool file_exists)
        {
            if (file_exists)
            {
                lblFileExists.Text = "File Exists";
                btnLoad.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnLoad.Enabled = false;
                btnDelete.Enabled = false;
                lblFileExists.Text = "File not found - does not exist";
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                FileUtil.WriteToXmlFile<TZDataList>(txtSaveFilename.Text, _tzdata);

                if (System.IO.File.Exists(txtSaveFilename.Text))
                {
                    SetFileExist(true);
                    MessageBox.Show(this, "File saved OK:" + Environment.NewLine + txtSaveFilename.Text, "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                MessageBox.Show(this, "File not saved!" + Environment.NewLine + txtSaveFilename.Text, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                lblFileExists.Text = "File not found - does not exist";
                if (MessageBox.Show(this, "File saved error!" + Environment.NewLine + txtSaveFilename.Text + Environment.NewLine + Environment.NewLine + "See full error details?", "File Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    MessageBox.Show(this, "Full Error:" + Environment.NewLine + ex.ToString(), "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            SetFileExist(false);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            TZDataList testLoad = null;
            string errString = "";
            try
            {
                if (System.IO.File.Exists(txtSaveFilename.Text))
                {
                    // this should aways be true. If not then ... well I just don't knwow what to say. 
                    testLoad = FileUtil.ReadFromXmlFile<TZDataList>(txtSaveFilename.Text);
                    if (testLoad != null)
                    {
                        // we have loaded some data! How about that!
                        if (testLoad.TimeZoneData.Count > 0)
                        {
                            _tzdata = testLoad;
                            _is_file_loaded = true;

                            MessageBox.Show(this, "File loaded OK", "File Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                System.Diagnostics.Trace.WriteLine("Error loading file " + txtSaveFilename.Text + Environment.NewLine + ex.ToString());

                errString = Environment.NewLine + ex.ToString();
            }

            if (string.IsNullOrWhiteSpace(errString))
            {
                MessageBox.Show(this, "File did not load OK, no data in file", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show(this, "Error loading file. Do you want to see the full error?", "Load Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)== DialogResult.Yes)
                {
                    MessageBox.Show(this, "Full Error: " + errString, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(txtSaveFilename.Text))
            {
                // this should always be true and we should always be here
                try
                {
                    System.IO.File.Delete(txtSaveFilename.Text);
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show(this, "Error deleting file. Do you want to see the full error?", "Delete Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        MessageBox.Show(this, "Full Error: " + ex.ToString(), "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            SetFileExist(Does_file_exist());
        }
        private bool Does_file_exist()
        {
            bool does_file_exist = false;
            if (!string.IsNullOrWhiteSpace(txtSaveFilename.Text))
            {
                try { does_file_exist = System.IO.File.Exists(txtSaveFilename.Text); }
                catch { } // Don’t really care here. What can I do anyway? I’m just a lowly programmer and here we have some error when you are running this program. You need to take responsibility for the choices this program takes while you are running it. See you on the flipside. 
            }
            return does_file_exist;
        }
        private void btnDefault_Click(object sender, EventArgs e)
        {
            txtSaveFilename.Text = _save_filename;
            SetFileExist(Does_file_exist());
        }
    }
}
