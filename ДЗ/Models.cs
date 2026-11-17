using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    [Table("Books")]
    public class Book
    {
        [Key]
        [Column("udc")]
        public string UDC { get; set; } = "";
        [Column("author")]
        public string Author { get; set; } = "";
        [Column("title")]
        public string Title { get; set; } = "";
        [Column("year")]
        public int Year { get; set; } = DateTime.Now.Year;
        [Column("total")]
        public int Total { get; set; }
        [Column("now_available")]
        public int Now { get; set; }
        public Book() { }
        public Book(string udc, string author, string title, int year, int total)
        {
            UDC = udc;
            Author = author;
            Title = title;
            Year = year;
            Total = total;
            Now = total;
        }
        public override string ToString()
        {
            return UDC + " | " + Author + " | " + Title + " | " + Year + " | Всего: " + Total + " | Доступно: " + Now;
        }
    }
    [Serializable]
    [Table("Readers")]
    public class Reader
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("full_name")]
        public string FullName { get; set; } = "";
        [Column("ticket_number")]
        public string TicketNumber { get; set; } = "";
        [Column("phone")]
        public string Phone { get; set; } = "";
        [Column("email")]
        public string Email { get; set; } = "";
        [Column("registered_at")]
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        public Reader() { }

        public Reader(int id, string fullName, string ticket, string phone, string email)
        {
            Id = id;
            FullName = fullName;
            TicketNumber = ticket;
            Phone = phone;
            Email = email;
        }
        public override string ToString()
        {
            string s;
            if (IsActive == true) s = "активен";
            else s = "заблокирован";
            return Id + " | " + FullName + " | " + TicketNumber + " | " + Phone + " | " + s;
        }
    }
    public enum LoanStatus { Active, Overdue, Returned }
    [Serializable]
    [Table("Loans")]
    public class Loan
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("book_udc")]
        public string BookUdc { get; set; } = "";
        [Column("reader_id")]
        public int ReaderId { get; set; }
        [Column("issued_at")]
        public DateTime IssuedAt { get; set; } = DateTime.Now;
        [Column("due_date")]
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);
        [Column("returned_at")]
        public DateTime? ReturnedAt { get; set; }
        [ForeignKey("BookUdc")]
        public Book Book { get; set; }
        [ForeignKey("ReaderId")]
        public Reader Reader { get; set; }
        [NotMapped]
        public bool IsReturned
        {
            get { return ReturnedAt.HasValue; }
        }
        [NotMapped] // без колонки
        public int DaysOverdue
        {
            get
            {
                if (IsReturned == true) return 0;
                if (DateTime.Now.Date <= DueDate.Date) return 0;
                TimeSpan diff = DateTime.Now.Date - DueDate.Date;
                return diff.Days;
            }
        }
        [NotMapped]
        public LoanStatus Status
        {
            get
            {
                if (IsReturned == true) return LoanStatus.Returned;
                if (DaysOverdue > 0) return LoanStatus.Overdue;
                return LoanStatus.Active;
            }
        }
        public override string ToString()
        {
            string s = "#" + Id + " | " + BookUdc + " | Читатель #" + ReaderId + " | Срок " + DueDate.ToString("dd.MM.yyyy") + " | " + Status.ToString();
            if (IsReturned == false)
            {
                s = s + " | Просрочка " + DaysOverdue + " дн.";
            }
            return s;
        }
    }
}
