using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Library
{
    public partial class FormIssueLoan : Form
    {
        public List<Reader> AllReaders;
        public int SelectedReaderId = 0;
        public int Days = 14;
        public FormIssueLoan()
        {
            InitializeComponent();
        }

        private void FormIssueLoan_Load(object sender, EventArgs e)
        {
            cmbReader.DataSource = AllReaders;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbReader.SelectedItem == null) { MessageBox.Show("Выберите читателя"); return; }
            SelectedReaderId = (cmbReader.SelectedItem as Reader).Id;
            Days = (int)numDays.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}