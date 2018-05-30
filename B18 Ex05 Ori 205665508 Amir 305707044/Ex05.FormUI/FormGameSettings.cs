using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex05.FormUI
{
    public enum eBoardSize { Small = 6, Medium = 8, Large = 10 };

    public partial class FormGameSettings : Form
    {
        private const string k_SettingsOk = "Settings were set, Enjoy Your Game!";
        private const string k_SettingsBad = "Invalid game settings, Try Again!";

        private eBoardSize m_Size = eBoardSize.Medium; //Default value for boardsize
        bool m_SettingsOk = false;

        public FormGameSettings()
        {
            InitializeComponent();
        }

        public string Player1Name
        {
            get { return textBox1.Text; }
        }

        public string Player2Name
        {
            get { return textBox2.Text; }
        }

        public bool DoublePlayer
        {
            get { return checkBox1.Checked; }
        }
        public eBoardSize BoardSize
        {
            get { return m_Size; }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                textBox2.Text = string.Empty;
                textBox2.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
                textBox2.Text = "[Computer]";
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            m_Size = eBoardSize.Small;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            m_Size = eBoardSize.Medium;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            m_Size = eBoardSize.Large;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                m_SettingsOk = true;
                MessageBox.Show(k_SettingsOk);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                m_SettingsOk = false;
                MessageBox.Show(k_SettingsBad);
                this.DialogResult = DialogResult.Retry;
            }
        }

        private bool checkInput()
        {
            bool isInputValid = true;

            if (Player1Name == string.Empty)
            {
                isInputValid = false;
                if (DoublePlayer && Player2Name == string.Empty)
                {
                    isInputValid = false;
                }
            }
            else
            {
                checkInput();
            }

            return isInputValid;
        }

        private void FormGameSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }
    }
}
