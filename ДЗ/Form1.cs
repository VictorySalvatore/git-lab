using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Library
{
    public partial class Form1 : Form
    {
        private LibraryService m;
        public Form1()
        {
            InitializeComponent();
            m = new LibraryManager();
            m.DataChanged += M_DataChanged;
            try
            {
                if (File.Exists("library.dat")) m.Load("library.dat");
                else m.InitializeDefaultData();
            }
            catch { m.InitializeDefaultData(); }
            cmbViewMode.SelectedIndex = 0;
            cmbViewMode.SelectedIndexChanged += cmbViewMode_SelectedIndexChanged;
            RefreshAll();
        }
        private void RefreshAll()
        {
            listBox1.Items.Clear();
            string mode = cmbViewMode.Text;
            if (mode == "Книги")
            {
                var books = m.Books;
                foreach (var b in books) listBox1.Items.Add(b.ToString());
                this.Text = $"Библиотека - Всего книг: {books.Count}";
            }
            else if (mode == "Читатели")
            {
                var readers = m.Readers;
                foreach (var r in readers) listBox1.Items.Add(r.ToString());
                this.Text = $"Библиотека - Всего читателей: {readers.Count}";
            }
            else if (mode == "Выдачи")
            {
                var loans = m.Loans;
                foreach (var l in loans) listBox1.Items.Add(l.ToString());
                this.Text = $"Библиотека - Всего выдач: {loans.Count}";
            }
            textBox1.Clear(); 
            textBox2.Clear(); 
            textBox3.Clear();
            textBox4.Clear(); 
            textBox5.Clear(); 
            textBox6.Clear();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            if (cmbViewMode.Text == "Книги")
            {
                var b = m.Books[listBox1.SelectedIndex];
                textBox1.Text = b.UDC;
                textBox2.Text = b.Author;
                textBox3.Text = b.Title;
                textBox4.Text = b.Year.ToString();
                textBox5.Text = b.Total.ToString();
                textBox6.Text = b.Now.ToString();
            }
            else if (cmbViewMode.Text == "Читатели")
            {
                var r = m.Readers[listBox1.SelectedIndex];
                textBox1.Text = r.Id.ToString();
                textBox2.Text = r.FullName;
                textBox3.Text = r.TicketNumber;
                textBox4.Text = r.Phone;
                textBox5.Text = r.Email;
                textBox6.Text = r.IsActive ? "Активен" : "Заблокирован";
            }
            else if (cmbViewMode.Text == "Выдачи")
            {
                var l = m.Loans[listBox1.SelectedIndex];
                textBox1.Text = l.Id.ToString();
                textBox2.Text = l.BookUdc;
                textBox3.Text = l.ReaderId.ToString();
                textBox4.Text = l.DueDate.ToString("dd.MM.yyyy");
                textBox5.Text = l.Status.ToString();
                textBox6.Text = l.DaysOverdue.ToString();
            }
        }
        private void M_DataChanged(object sender, EventArgs e)
        {
            RefreshAll();
        }
        // 8 кнопок
        private void button1_Click(object sender, EventArgs e) // Добавить
        {
            if (cmbViewMode.Text == "Книги")
            {
                try
                {
                    var b = new Book(textBox1.Text, textBox2.Text, textBox3.Text, int.Parse(textBox4.Text), int.Parse(textBox5.Text));
                    m.AddBook(b);
                }
                catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
            }
            else if (cmbViewMode.Text == "Читатели")
            {
                FormAddReader form = new FormAddReader();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    m.AddReader(form.NewReader);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e) // Обновить
        {
            try
            {
                if (cmbViewMode.Text == "Книги" && listBox1.SelectedIndex != -1)
                {
                    var b = m.Books[listBox1.SelectedIndex];
                    b.Author = textBox2.Text; 
                    b.Title = textBox3.Text;
                    b.Year = int.Parse(textBox4.Text);
                    b.Total = int.Parse(textBox5.Text);
                    m.UpdateBook(b);
                    MessageBox.Show("Книга обновлена");
                }
                else if (cmbViewMode.Text == "Читатели" && listBox1.SelectedIndex != -1)
                {
                    var r = m.Readers[listBox1.SelectedIndex];
                    r.FullName = textBox2.Text; 
                    r.TicketNumber = textBox3.Text;
                    r.Phone = textBox4.Text; 
                    r.Email = textBox5.Text;
                    m.UpdateReader(r);
                    MessageBox.Show("Читатель обновлен");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void button3_Click(object sender, EventArgs e) // Выдать
        {
            if (cmbViewMode.Text != "Книги" || listBox1.SelectedIndex == -1)
            { 
                MessageBox.Show("Выберите книгу на вкладке 'Книги'"); 
                return; 
            }
            var book = m.Books[listBox1.SelectedIndex];
            FormIssueLoan form = new FormIssueLoan();
            form.AllReaders = m.Readers;
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    m.IssueLoan(book.UDC, form.SelectedReaderId, form.Days);
                    MessageBox.Show("Успешно выдано!");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
        private void button4_Click(object sender, EventArgs e) // Вернуть
        {
            if (cmbViewMode.Text != "Выдачи" || listBox1.SelectedIndex == -1)
            { 
                MessageBox.Show("Переключитесь на 'Выдачи' и выберите строку"); 
                return; 
            }
            try
            {
                var loan = m.Loans[listBox1.SelectedIndex];
                m.ReturnLoan(loan.Id);
                MessageBox.Show("Книга возвращена");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void button5_Click(object sender, EventArgs e) // Удалить
        {
            if (listBox1.SelectedIndex == -1) 
            { 
                MessageBox.Show("Выберите строку"); 
                return; 
            }
            try
            {
                if (cmbViewMode.Text == "Книги") m.RemoveBook(m.Books[listBox1.SelectedIndex].UDC);
                else if (cmbViewMode.Text == "Читатели") m.RemoveReader(m.Readers[listBox1.SelectedIndex].Id);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog 
            { 
                Filter = "CSV|*.csv" 
            };
            if (d.ShowDialog() == DialogResult.OK) 
            { 
                m.ExportCsv(d.FileName); 
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog 
            { 
                Filter = "CSV|*.csv" 
            };
            if (d.ShowDialog() == DialogResult.OK) 
            { 
                m.ImportCsv(d.FileName); 
            }
        }
        private void button8_Click(object sender, EventArgs e) // Отчет
        {
            string report = m.BuildOverdueReportText();
            MessageBox.Show(report, "Отчёт о просрочках");
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Сохранить изменения перед выходом?", "Выход",
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try 
                {
                    m.Save("library.dat");
                    System.Threading.Tasks.Task.Run(() => m.SaveToDatabase()); // в фоновом потоке
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show("Ошибка при сохранении: " + ex.Message); 
                }
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        private void cmbViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshAll();
            if (cmbViewMode.Text == "Книги")
            {
                // для книг
                label1.Text = "УДК:";
                label2.Text = "Автор:";
                label3.Text = "Название:";
                label4.Text = "Год:";
                label5.Text = "Всего:";
                label6.Text = "Доступно:";
                label4.Visible = true; 
                textBox4.Visible = true;
                label5.Visible = true; 
                textBox5.Visible = true;
                label6.Visible = true; 
                textBox6.Visible = true;
            }
            else if (cmbViewMode.Text == "Читатели")
            {
                // для читателей
                label1.Text = "ID:";
                label2.Text = "ФИО:";
                label3.Text = "Билет:";
                label4.Text = "Телефон:";
                label5.Text = "Email:";
                label6.Text = "Статус:";
                label4.Visible = true; 
                textBox4.Visible = true;
                label5.Visible = true; 
                textBox5.Visible = true;
                label6.Visible = true; 
                textBox6.Visible = true;
            }
            else if (cmbViewMode.Text == "Выдачи")
            {
                // для выдач
                label1.Text = "ID выдачи:";
                label2.Text = "Книга (УДК):";
                label3.Text = "ID читателя:";
                label4.Text = "Срок сдачи:";
                label5.Text = "Статус:";
                label6.Text = "Просрочка (дн.):";
                label4.Visible = true; 
                textBox4.Visible = true;
                label5.Visible = true; 
                textBox5.Visible = true;
                label6.Visible = true; 
                textBox6.Visible = true;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}