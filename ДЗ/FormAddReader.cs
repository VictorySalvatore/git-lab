using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Library
{
    public partial class FormAddReader : Form
    {
        public FormAddReader()
        {
            InitializeComponent();
        }
        public Reader NewReader = null; // Сюда сохраним результат
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtFio.Text == "") { MessageBox.Show("Введите ФИО"); return; }
            NewReader = new Reader(0, txtFio.Text, txtTicket.Text, txtPhone.Text, txtEmail.Text);
            this.DialogResult = DialogResult.OK; // Закрыть окно с успехом
            this.Close();
        }
        private void FormAddReader_Load(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
