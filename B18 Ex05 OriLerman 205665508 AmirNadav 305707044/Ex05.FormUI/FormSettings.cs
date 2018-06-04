using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ex05.CheckersLogic;

namespace Ex05.FormUI
{
    public partial class FormSettings : Form
    {
        private const eBoardSizeOptions m_DefaultSize = eBoardSizeOptions.MediumBoard;

        private eBoardSizeOptions m_Size = eBoardSizeOptions.MediumBoard; // Default value for boardsize

        public FormSettings()
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

        public eBoardSizeOptions BoardSize
        {
            get
            {
                eBoardSizeOptions returnedSize;
                if (this.DialogResult == DialogResult.Cancel)
                {
                    returnedSize = m_DefaultSize;
                }
                else
                {
                    returnedSize = m_Size;
                }

                return returnedSize;
            }
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
            m_Size = eBoardSizeOptions.SmallBoard;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            m_Size = eBoardSizeOptions.MediumBoard;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            m_Size = eBoardSizeOptions.LargeBoard;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}
