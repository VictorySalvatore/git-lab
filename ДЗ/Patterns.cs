using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Library
{
    // Порождающие
    public class Logger
    {
        private static Logger inst; // Singleton
        private Logger()
        {
        }
        public static Logger I
        {
            get
            {
                if (inst == null)
                {
                    inst = new Logger();
                }
                return inst; // единственный экземпляр
            }
        }
        public void Log(string level, string msg)
        {
            string line = "[" + DateTime.Now.ToString("HH:mm:ss") + "] [" + level + "] " + msg + "\n";
            File.AppendAllText("library.log", line);
        }
    }
    public abstract class LibraryFactory // Factory method
    {
        public abstract Book NewBook(string udc, string author, string title, int year, int total);
        public abstract Reader NewReader(int id, string fullName, string ticket, string phone, string email);
    }

    public class DefLibraryFactory : LibraryFactory // Конкретная фабрика
    {
        public override Book NewBook(string udc, string author, string title, int year, int total)
        {
            Book b = new Book(udc, author, title, year, total);
            return b;
        }

        public override Reader NewReader(int id, string fullName, string ticket, string phone, string email)
        {
            Reader r = new Reader(id, fullName, ticket, phone, email);
            return r;
        }
    }
    public class LoanBuilder // Builder
    {
        private Loan loan = new Loan(); // Пустая выдача + заполнение
        public LoanBuilder WithId(int id)
        {
            loan.Id = id;
            return this;
        }
        public LoanBuilder ForBook(string udc)
        {
            loan.BookUdc = udc;
            return this;
        }
        public LoanBuilder ForReader(int readerId)
        {
            loan.ReaderId = readerId;
            return this;
        }
        public LoanBuilder Term(int days)
        {
            loan.DueDate = DateTime.Now.AddDays(days);
            return this;
        }
        public Loan Build()
        {
            if (loan.BookUdc == "")
            {
                throw new InvalidOperationException("Не указана книга");
            }
            if (loan.ReaderId <= 0)
            {
                throw new InvalidOperationException("Не указан читатель");
            }
            return loan;
        }
    }
    // Поведенческие
    public interface PenaltyStrategy // Strategy
    {
        decimal Calc(int days);
        string Name { get; }
    }
    public class NoPenalty : PenaltyStrategy
    {
        public string Name
        {
            get { return "Без штрафа"; }
        }
        public decimal Calc(int days)
        {
            return 0;
        }
    }
    public class ProgressivePenalty : PenaltyStrategy
    {
        public string Name
        {
            get { return "Прогрессивная"; }
        }

        public decimal Calc(int days)
        {
            if (days <= 0) return 0;

            if (days <= 7) return days * 5;

            if (days <= 30)
            {
                decimal sum = 7 * 5;
                sum += (days - 7) * 10;
                return sum;
            }
            decimal res = 7 * 5 + 23 * 10;
            res += (days - 30) * 20;
            return res;
        }
    }
    public interface Iter<T> // Iterator
    {
        bool HasNext();
        T Next();
    }
    public class LoansByReader : Iter<Loan>
    {
        private List<Loan> items = new List<Loan>();
        private int pos = 0;
        public LoansByReader(List<Loan> all, int readerId)
        {
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].ReaderId == readerId)
                {
                    items.Add(all[i]);
                }
            }
        }
        public bool HasNext()
        {
            if (pos < items.Count) return true;
            return false;
        }
        public Loan Next()
        {
            if (pos < items.Count)
            {
                Loan l = items[pos];
                pos += 1;
                return l;
            }
            throw new InvalidOperationException("Больше нет элементов");
        }
    }
    public abstract class IssueHandler // Chain of Responsibility
    {
        protected IssueHandler next;
        public IssueHandler Next(IssueHandler n)
        {
            next = n;
            return n;
        }
        public abstract string Check(Book b, Reader r);
    }
    public class BookCheck : IssueHandler
    {
        public override string Check(Book b, Reader r)
        {
            if (b == null) return "Книга не найдена";
            if (b.Now <= 0) return "Нет экземпляров";
            if (next != null) return next.Check(b, r);
            return null;
        }
    }
    public class ReaderCheck : IssueHandler
    {
        public override string Check(Book b, Reader r)
        {
            if (r == null) return "Читатель не найден";
            if (r.IsActive == false) return "Читатель заблокирован";
            if (next != null) return next.Check(b, r);
            return null;
        }
    } 
    // Структурные
    public interface Report // Proxy
    {
        List<Loan> Build();
    }
    public class RealReport : Report
    {
        private List<Loan> loans;

        public RealReport(List<Loan> l)
        {
            loans = l;
        }
        public List<Loan> Build()
        {
            List<Loan> result = new List<Loan>();
            for (int i = 0; i < loans.Count; i++)
            {
                Loan l = loans[i];
                if (l.IsReturned == false && l.DaysOverdue > 0)
                {
                    result.Add(l);
                }
            }
            return result;
        }
    }
    public class CachedReport : Report
    {
        private Report real;
        private List<Loan> cache;
        private DateTime cacheTime;

        public CachedReport(Report r)
        {
            real = r;
        }
        public List<Loan> Build()
        {
            bool needRecalc = false;
            if (cache == null)
            {
                needRecalc = true;
            }
            else
            {
                TimeSpan diff = DateTime.Now - cacheTime;
                if (diff.TotalSeconds > 3) needRecalc = true;
            }
            if (needRecalc == true)
            {
                cache = real.Build();
                cacheTime = DateTime.Now;
            }
            return cache;
        }
    }
    public interface BooksCsv // Adapter
    {
        string Ser(List<Book> books);
        List<Book> Des(string csv);
    }
    public class BookCsv : BooksCsv
    {
        public string Ser(List<Book> books)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("УДК;Автор;Название;Год;Всего;Доступно");

            for (int i = 0; i < books.Count; i++)
            {
                Book b = books[i];
                string line = b.UDC + ";" + b.Author + ";" + b.Title + ";" +
                              b.Year + ";" + b.Total + ";" + b.Now;
                sb.AppendLine(line);
            }
            return sb.ToString();
        }
        public List<Book> Des(string csv)
        {
            List<Book> result = new List<Book>();
            string[] lines = csv.Split('\n');
            for (int i = 1; i < lines.Length; i++) // 0 заголовок
            {
                string line = lines[i].Trim();
                if (line == "") continue;
                string[] parts = line.Split(';');
                if (parts.Length < 6) continue;
                Book b = new Book();
                b.UDC = parts[0];
                b.Author = parts[1];
                b.Title = parts[2];
                int y;
                if (int.TryParse(parts[3], out y) == true) b.Year = y;
                int tot;
                if (int.TryParse(parts[4], out tot) == true) b.Total = tot;
                int now;
                if (int.TryParse(parts[5], out now) == true) b.Now = now;
                result.Add(b);
            }
            return result;
        }
    }
    public interface ReportOutput // Bridge
    {
        void Write(string text);
    }
    public class MessageBoxOutput : ReportOutput
    {
        public void Write(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "Отчёт");
        }
    }
    public abstract class Reporting
    {
        protected ReportOutput output;
        public Reporting(ReportOutput o)
        {
            output = o;
        }
        public abstract void Making(List<Loan> loans);
    }
    public class BridgeReport : Reporting
    {
        public BridgeReport(ReportOutput o) : base(o) { }
        public override void Making(List<Loan> loans)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== Просрочки ===");
            int count = 0;
            for (int i = 0; i < loans.Count; i++)
            {
                Loan l = loans[i];
                if (l.IsReturned == false && l.DaysOverdue > 0)
                {
                    sb.AppendLine(l.ToString());
                    count += 1;
                }
            }
            sb.AppendLine("Итого: " + count);
            output.Write(sb.ToString());
        }
    }
}
