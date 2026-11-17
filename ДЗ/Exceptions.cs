using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class LibraryException : Exception
    {
        public LibraryException(string message) : base(message) { }
    }
    public class BookNotAvailableException : LibraryException
    {
        public BookNotAvailableException(string udc) : base("Нет экземпляров " + udc) { }
    }
    public class ReaderNotFoundException : LibraryException
    {
        public ReaderNotFoundException(int id) : base("Читатель #" + id + " не найден") { }
    }
    public class DuplicateLoanException : LibraryException
    {
        public DuplicateLoanException(string udc, int readerId)
            : base("Читатель #" + readerId + " уже держит " + udc) { }
    }
}
