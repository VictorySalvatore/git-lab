using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public interface LibraryService 
    {
        event EventHandler DataChanged; // ув. о событиях
        List<Book> Books { get; }
        List<Reader> Readers { get; }
        List<Loan> Loans { get; }
        Book FindBook(string udc);
        Reader FindReader(int id);
        void AddBook(Book b);
        void UpdateBook(Book b);
        void RemoveBook(string udc);
        void AddReader(Reader r);
        void UpdateReader(Reader r);
        void RemoveReader(int id);
        Loan IssueLoan(string udc, int readerId, int days = 14);
        void ReturnLoan(int id);
        decimal Penalty(int id);
        List<Loan> Overdue();
        string BuildOverdueReportText();
        void Save(string path);
        void Load(string path);
        void ExportCsv(string path);
        void ImportCsv(string path);
        void InitializeDefaultData();
        void SaveToDatabase();
    }
    public class LibraryManager : LibraryService
    {
        private List<Book> books = new List<Book>();
        private List<Reader> readers = new List<Reader>();
        private List<Loan> loans = new List<Loan>();
        private LibraryFactory factory = new DefLibraryFactory();
        private PenaltyStrategy penalty = new ProgressivePenalty();
        private int nextReaderId = 1;
        private int nextLoanId = 1;
        public event EventHandler DataChanged;
        public List<Book> Books // копии для защиты
        {
            get
            {
                List<Book> res = new List<Book>();
                for (int i = 0; i < books.Count; i++) res.Add(books[i]);
                return res;
            }
        }
        public List<Reader> Readers
        {
            get
            {
                List<Reader> res = new List<Reader>();
                for (int i = 0; i < readers.Count; i++) res.Add(readers[i]);
                return res;
            }
        }
        public List<Loan> Loans
        {
            get
            {
                List<Loan> res = new List<Loan>();
                for (int i = 0; i < loans.Count; i++) res.Add(loans[i]);
                return res;
            }
        }
        // Книги
        public Book FindBook(string udc)
        {
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].UDC == udc) return books[i];
            }
            return null;
        }
        public void AddBook(Book b)
        {
            if (b == null) return;
            Book exist = this.FindBook(b.UDC);
            if (exist != null) throw new LibraryException("Книга с УДК " + b.UDC + " уже есть");
            books.Add(b);
            Logger.I.Log("INFO", "Добавили книгу " + b.UDC);
            OnChanged();
        }

        public void UpdateBook(Book b)
        {
            Book exist = this.FindBook(b.UDC);
            if (exist == null) throw new LibraryException("Книга не найдена");
            int oldNow = exist.Now;
            exist.Author = b.Author;
            exist.Title = b.Title;
            exist.Year = b.Year;
            int diff = b.Total - exist.Total;
            exist.Total = b.Total;
            exist.Now = oldNow + diff;
            if (exist.Now < 0) exist.Now = 0;
            if (exist.Now > exist.Total) exist.Now = exist.Total;
            OnChanged();
        }

        public void RemoveBook(string udc)
        {
            Book b = this.FindBook(udc);
            if (b == null) throw new LibraryException("Книга не найдена");
            books.Remove(b);
            OnChanged();
        }
        // Читатели
        public Reader FindReader(int id)
        {
            for (int i = 0; i < readers.Count; i++)
            {
                if (readers[i].Id == id) return readers[i];
            }
            return null;
        }
        public void AddReader(Reader r)
        {
            if (r == null) return;
            r.Id = nextReaderId;
            nextReaderId = nextReaderId + 1;
            readers.Add(r);
            OnChanged();
        }
        public void UpdateReader(Reader r)
        {
            Reader exist = this.FindReader(r.Id);
            if (exist == null) throw new ReaderNotFoundException(r.Id);
            exist.FullName = r.FullName;
            exist.Phone = r.Phone;
            exist.Email = r.Email;
            exist.IsActive = r.IsActive;
            OnChanged();
        }
        public void RemoveReader(int id)
        {
            Reader r = this.FindReader(id);
            if (r == null) throw new ReaderNotFoundException(id);
            readers.Remove(r);
            OnChanged();
        }
        // Выдачи
        public Loan IssueLoan(string udc, int readerId, int days = 14)
        {
            Book b = this.FindBook(udc);
            Reader r = this.FindReader(readerId);
            IssueHandler h = new BookCheck();
            h.Next(new ReaderCheck());
            string error = h.Check(b, r);
            if (error != null) throw new LibraryException(error);
            for (int i = 0; i < loans.Count; i++)
            {
                if (loans[i].BookUdc == udc && loans[i].ReaderId == readerId && loans[i].IsReturned == false)
                    throw new DuplicateLoanException(udc, readerId);
            }
            Loan loan = new LoanBuilder()
                .WithId(nextLoanId)
                .ForBook(udc)
                .ForReader(readerId)
                .Term(days)
                .Build();
            nextLoanId += 1;
            loans.Add(loan);
            b.Now -= 1; // -1 доступ. книга
            Logger.I.Log("INFO", "Выдали " + udc + " читателю " + readerId);
            OnChanged();
            return loan;
        }
        public void ReturnLoan(int id)
        {
            Loan l = null;
            for (int i = 0; i < loans.Count; i++)
            {
                if (loans[i].Id == id) 
                { 
                    l = loans[i]; 
                    break; 
                }
            }
            if (l == null) throw new LibraryException("Выдача не найдена");
            if (l.IsReturned) throw new LibraryException("Эта книга уже возвращена");
            l.ReturnedAt = DateTime.Now;
            Book b = this.FindBook(l.BookUdc);
            if (b != null && b.Now < b.Total) b.Now += 1;
            OnChanged();
        }
        public decimal Penalty(int id)
        {
            Loan l = null;
            for (int i = 0; i < loans.Count; i++)
            {
                if (loans[i].Id == id) 
                { 
                    l = loans[i]; 
                    break; 
                }
            }
            if (l == null) throw new LibraryException("Выдача не найдена");
            return penalty.Calc(l.DaysOverdue);
        }
        // отчет
        public List<Loan> Overdue()
        {
            Report report = new CachedReport(new RealReport(loans));
            return report.Build();
        }
        public string BuildOverdueReportText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== Просрочки ===");
            sb.AppendLine("Сформирован: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
            List<Loan> ov = this.Overdue();
            ov.Sort(delegate (Loan a, Loan b) // по убыв.
            {
                return b.DaysOverdue.CompareTo(a.DaysOverdue);
            });
            int count = 0;
            foreach (Loan l in ov)
            {
                Reader r = this.FindReader(l.ReaderId);
                Book bk = this.FindBook(l.BookUdc);
                string fio = "?";
                if (r != null)
                {
                    fio = r.FullName;
                }
                string title = "?";
                if (bk != null)
                {
                    title = bk.Title;
                }
                decimal penSum = penalty.Calc(l.DaysOverdue);
                string penStr = penSum.ToString("C"); // деньги
                string line = fio + " | " + title + " [" + l.BookUdc + "] | " + l.DaysOverdue + " дн. | штраф " + penStr;
                sb.AppendLine(line);
                count += 1;
            }
            sb.AppendLine("Всего просрочено: " + count);
            return sb.ToString();
        }
        // Файлы
        public void Save(string path)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.Create);
                try 
                { // 3 списка в массив
                    object[] d = new object[] { books, readers, loans };
                    bf.Serialize(fs, d);
                }
                finally
                {
                    fs.Close();
                }
            }
            catch (Exception ex) { throw new LibraryException("Ошибка сохранения: " + ex.Message); }
        }
        public void SaveToDatabase()
        {
            using (var db = new LibraryDbContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM Loans");
                db.Database.ExecuteSqlCommand("DELETE FROM Books");
                db.Database.ExecuteSqlCommand("DELETE FROM Readers");
                db.Books.AddRange(books);
                db.Readers.AddRange(readers);
                db.Loans.AddRange(loans);
                db.SaveChanges();
            }
        }
        public void Load(string path) // загрузка из бин. файла
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.Open);
                object[] d;
                try
                {
                    d = (object[])bf.Deserialize(fs);
                }
                finally
                {
                    fs.Close();
                }
                books = (List<Book>)d[0];
                readers = (List<Reader>)d[1];
                loans = (List<Loan>)d[2];
                if (readers.Count > 0)
                {
                    nextReaderId = readers[readers.Count - 1].Id + 1;
                }
                if (loans.Count > 0)
                {
                    nextLoanId = loans[loans.Count - 1].Id + 1;
                }
                OnChanged();
            }
            catch (Exception ex) { throw new LibraryException("Ошибка загрузки: " + ex.Message); }
        }
        public void ExportCsv(string path)
        {
            BookCsv adapter = new BookCsv();
            File.WriteAllText(path, adapter.Ser(books), Encoding.UTF8);
        }
        public void ImportCsv(string path)
        {
            BookCsv adapter = new BookCsv();
            books = adapter.Des(File.ReadAllText(path, Encoding.UTF8));
            OnChanged();
        }
        public void InitializeDefaultData()
        {
            books.Clear(); readers.Clear(); loans.Clear();
            nextReaderId = 1; nextLoanId = 1;
            books.Add(new Book("004.4", "Иванов И.И.", "Программирование на C#", 2020, 5));
            books.Add(new Book("517.2", "Петров П.П.", "Математический анализ", 2019, 3));
            books.Add(new Book("53.01", "Джордж С.С.", "Песнь льда и пламени", 2021, 2));
            books.Add(new Book("82-31", "Смирнова А.А.", "Русская литература", 2018, 4));
            books.Add(new Book("61", "Козлова Е.Е.", "Гарри Поттер", 2022, 1));
            Reader r1 = factory.NewReader(nextReaderId, "Алексей Мирный", "T-001", "+7 900 111-22-33", "a@mail.ru");
            nextReaderId++;
            Reader r2 = factory.NewReader(nextReaderId, "Мария Половинчик", "T-002", "+7 900 666-33-44", "m@mail.ru");
            nextReaderId++;
            readers.Add(r1); 
            readers.Add(r2);
            this.IssueLoan("004.4", 1, -5);
            this.IssueLoan("517.2", 2, 10);
            OnChanged();
        }
        protected void OnChanged()
        {
            if (this.DataChanged != null) this.DataChanged(this, EventArgs.Empty);
        }
    }
}