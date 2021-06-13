using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SwissArmyKnife.Forms
{
    public partial class GameSelect : Form
    {
        public string selected { get; private set; }
        public GameSelect()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selected == default(string))
            {
                MessageBox.Show("To proceed, you must select an option.", "Select an option!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) => selected = "HGSS";
        private void radioButton2_CheckedChanged(object sender, EventArgs e) => selected = "BW";
        private void radioButton3_CheckedChanged(object sender, EventArgs e) => selected = "B2W2";
    }
}
