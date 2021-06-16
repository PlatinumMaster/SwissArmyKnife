using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SwissArmyKnife.Forms
{
    public partial class ScriptPluginSelect : Form
    {
        public int[][] SelectedPlugins { get => _SelectedPlugins.ToArray(); }
        List<int[]> _SelectedPlugins = new List<int[]>();
        Dictionary<int, int[]> IndexToPluginIndex = new Dictionary<int, int[]>()
        {
            [0] = new int[] { 50 },
            [1] = new int[] { 51 },
            [2] = new int[] { 52 },
            [3] = new int[] { 53 },
            [4] = new int[] { 54 },
            [5] = new int[] { 55, 56 },
            [6] = new int[] { 55, 57 },
            [7] = new int[] { 58, 59 },
            [8] = new int[] { 61 },
            [9] = new int[] { 62 },
            [10] = new int[] { 63 },
            [11] = new int[] { 64 },
            [12] = new int[] { 65 },
            [13] = new int[] { 66 },
            [14] = new int[] { 67 },
            [15] = new int[] { 68 },

        };

        public ScriptPluginSelect()
        {
            InitializeComponent();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; ++i)
                if (checkedListBox1.GetItemChecked(i))
                    _SelectedPlugins.Add(IndexToPluginIndex[i]);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
